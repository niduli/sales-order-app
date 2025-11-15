using Infrastructure.Data;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------
// Add Controllers (VERY IMPORTANT)
// ------------------------------------------
builder.Services.AddControllers();

// ------------------------------------------
// Add DbContext (SQL Server + EF Core)
// ------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------------------------
// Dependency Injection (Repositories)
// ------------------------------------------
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ------------------------------------------
// Dependency Injection (Services)
// ------------------------------------------
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ItemService>();

// ------------------------------------------
// AutoMapper
// ------------------------------------------
builder.Services.AddAutoMapper(typeof(Program));

// ------------------------------------------
// CORS for React frontend
// ------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:5173");
        });
});

// ------------------------------------------
// Swagger
// ------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------------------------------
// Middlewares
// ------------------------------------------
app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ------------------------------------------
// Map Controllers (VERY IMPORTANT)
// ------------------------------------------
app.MapControllers();

// ------------------------------------------
// Seed the database
// ------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Seed(db);
}

app.Run();
