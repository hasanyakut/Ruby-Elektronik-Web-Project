using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<OrderDbContext>(options =>
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
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    context.Database.EnsureCreated();
}

// CRUD Endpoints
app.MapGet("/orders", async (OrderDbContext context) =>
{
    var orders = await context.Orders.ToListAsync();
    return Results.Ok(orders);
});

app.MapGet("/orders/{id}", async (int id, OrderDbContext context) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapPost("/orders", async (Order order, OrderDbContext context) =>
{
    order.CreatedAt = DateTime.UtcNow;
    order.TotalPrice = order.UnitPrice * order.Quantity;
    context.Orders.Add(order);
    await context.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}", order);
});

app.MapPut("/orders/{id}", async (int id, Order updatedOrder, OrderDbContext context) =>
{
    var order = await context.Orders.FindAsync(id);
    if (order == null) return Results.NotFound();
    
    order.UserId = updatedOrder.UserId;
    order.ProductId = updatedOrder.ProductId;
    order.Quantity = updatedOrder.Quantity;
    order.UnitPrice = updatedOrder.UnitPrice;
    order.TotalPrice = updatedOrder.UnitPrice * updatedOrder.Quantity;
    order.Status = updatedOrder.Status;
    order.Notes = updatedOrder.Notes;
    order.ProductName = updatedOrder.ProductName;
    order.UserName = updatedOrder.UserName;
    order.UpdatedAt = DateTime.UtcNow;
    
    await context.SaveChangesAsync();
    return Results.Ok(order);
});

app.MapDelete("/orders/{id}", async (int id, OrderDbContext context) =>
{
    var order = await context.Orders.FindAsync(id);
    if (order == null) return Results.NotFound();
    
    context.Orders.Remove(order);
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.Run();
