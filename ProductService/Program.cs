using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

// Use CORS
app.UseCors();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    context.Database.EnsureCreated();
}

// CRUD Endpoints
app.MapGet("/products", async (ProductDbContext context) =>
{
    var products = await context.Products.Where(p => p.IsActive).ToListAsync();
    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (int id, ProductDbContext context) =>
{
    var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/products", async (Product product, ProductDbContext context) =>
{
    product.CreatedAt = DateTime.UtcNow;
    context.Products.Add(product);
    await context.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/products/{id}", async (int id, Product updatedProduct, ProductDbContext context) =>
{
    var product = await context.Products.FindAsync(id);
    if (product == null) return Results.NotFound();
    
    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Description = updatedProduct.Description;
    product.Category = updatedProduct.Category;
    product.IsActive = updatedProduct.IsActive;
    product.UpdatedAt = DateTime.UtcNow;
    
    await context.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", async (int id, ProductDbContext context) =>
{
    var product = await context.Products.FindAsync(id);
    if (product == null) return Results.NotFound();
    
    product.IsActive = false;
    product.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.Run();
