using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StatSanctum.API.Repositories;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Repositories;

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

builder.Services.AddSingleton<IJwtHelper, JwtHelper>();

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

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Needed for Google OAuth
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
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
    })
    // Cookie Authentication for Google OAuth
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    })
    // Google OAuth
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/signin-google"; // Must match Google's redirect URI
        options.SaveTokens = true; // Save tokens (access_token, id_token) in cookies

        // Request email and profile scopes
        options.Scope.Add("email");
        options.Scope.Add("profile");
    });

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("JWT", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser();
    });

    options.AddPolicy("Google", policy =>
    {
        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser();
    });
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

// Google OAuth endpoints(for browsers)
app.MapGet("/web/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/web/secure"
    });
});

app.MapGet("/web/secure", (HttpContext context) =>
{
    var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
    return $"Hello {email} (Google OAuth)!";
});

app.MapControllers();

app.Run();
