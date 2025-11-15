using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
using API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------
// CORS (Allow React & general use)
// ------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ------------------------------------------
// Add Controllers + FIX JSON CYCLIC LOOPS
// ------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ------------------------------------------
// Add DbContext (SQL Server + Migrations Assembly)
// ------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")
    )
);

// ------------------------------------------
// Dependency Injection - Repositories
// ------------------------------------------
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ------------------------------------------
// Dependency Injection - Services
// ------------------------------------------
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<PdfGenerator>();

// ------------------------------------------
// AutoMapper
// ------------------------------------------
builder.Services.AddAutoMapper(typeof(Program));

// ------------------------------------------
// Swagger
// ------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------------------------------
// Middleware
// ------------------------------------------
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// ------------------------------------------
// Seed Sample Data
// ------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Seed(db);
}

app.Run();
