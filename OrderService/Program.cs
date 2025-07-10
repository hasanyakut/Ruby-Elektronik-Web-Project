var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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



// Order Model
// In-memory order list
var orders = new List<Order>
{
    new Order(1, "Product 1", 2),
    new Order(2, "Product 2", 1)
};

// CRUD Endpoints
app.MapGet("/orders", () => orders);

app.MapGet("/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapPost("/orders", (Order order) =>
{
    orders.Add(order);
    return Results.Created($"/orders/{order.Id}", order);
});

app.MapPut("/orders/{id}", (int id, Order updatedOrder) =>
{
    var index = orders.FindIndex(o => o.Id == id);
    if (index == -1) return Results.NotFound();
    orders[index] = updatedOrder with { Id = id };
    return Results.Ok(updatedOrder);
});

app.MapDelete("/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);
    if (order is null) return Results.NotFound();
    orders.Remove(order);
    return Results.NoContent();
});

app.Run();

record Order(int Id, string ProductName, int Quantity);
