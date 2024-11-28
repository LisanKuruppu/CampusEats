using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<IDbContext, CampusEatsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IUserService with its implementation
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

    if (dbContext.Database.EnsureCreated())
    {
        // Seed initial data if the database is newly created
        SeedDatabase(dbContext);
    }
}

app.Run();

void SeedDatabase(IDbContext dbContext)
{
    // Add initial products
    if (!dbContext.Products.Any())
    {
        dbContext.Products.AddRange(new[]
        {
            new Product { Name = "Product 1", Description = "Description 1", Price = 10.00m, ImageUrl = "image1.png" },
            new Product { Name = "Product 2", Description = "Description 2", Price = 15.00m, ImageUrl = "image2.png" },
            new Product { Name = "Product 3", Description = "Description 3", Price = 20.00m, ImageUrl = "image3.png" }
        });
    }

    // Add initial customers
    if (!dbContext.Customers.Any())
    {
        dbContext.Customers.Add(new Customer { Name = "John Doe", Email = "john@example.com" });
    }

    dbContext.SaveChanges();
}
