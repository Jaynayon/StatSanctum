using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Repositories;
using Microsoft.OpenApi.Models;
using StatSanctum.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString)
    .EnableSensitiveDataLogging(); // Enable seeing sensitive data
});

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var entityTypes = new[] { typeof(Item), typeof(Rarity), typeof(ItemType), typeof(User) };
foreach(var type in entityTypes)
{
    // Register generic handler per entity type
    builder.Services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(typeof(GetByIdQuery<>).MakeGenericType(type), type), typeof(Handler<>).MakeGenericType(type));
    builder.Services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(typeof(GetAllQuery<>).MakeGenericType(type), typeof(IEnumerable<>).MakeGenericType(type)), typeof(Handler<>).MakeGenericType(type));
    builder.Services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(typeof(CreateCommand<>).MakeGenericType(type), type), typeof(Handler<>).MakeGenericType(type));
    builder.Services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(typeof(UpdateCommand<>).MakeGenericType(type), type), typeof(Handler<>).MakeGenericType(type));
    builder.Services.AddScoped(typeof(IRequestHandler<,>).MakeGenericType(typeof(DeleteCommand<>).MakeGenericType(type), typeof(bool)), typeof(Handler<>).MakeGenericType(type));
}

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()); 
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization : `Bearer Generated-JWT-Token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

// Add Authentication
var authority = builder.Configuration["Authentication:Authority"];
var audience = builder.Configuration["Authentication:Audience"];
var key = builder.Configuration["Authentication:SecretKey"] ?? "";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.Authority = authority;
        jwtOptions.Audience = audience;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authority,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(key))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
