using MediatR;
using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<Item>, ItemRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
