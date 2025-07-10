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

// User Model
// In-memory user list
var users = new List<User>
{
    new User(1, "Ahmet Yılmaz", "ahmet@example.com", UserType.Individual, null, "+90 555 123 4567"),
    new User(2, "ABC Elektronik Ltd. Şti.", "info@abcelektronik.com", UserType.Corporate, "ABC Elektronik Ltd. Şti.", "+90 212 555 0123")
};

// CRUD Endpoints
app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/users", (User user) =>
{
    user = user with { Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1 };
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var index = users.FindIndex(u => u.Id == id);
    if (index == -1) return Results.NotFound();
    users[index] = updatedUser with { Id = id };
    return Results.Ok(updatedUser);
});

app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    users.Remove(user);
    return Results.NoContent();
});

app.Run();

enum UserType
{
    Individual,
    Corporate
}

record User(int Id, string Name, string Email, UserType UserType, string? CompanyName, string PhoneNumber);
