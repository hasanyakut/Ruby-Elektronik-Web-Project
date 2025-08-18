using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<UserDbContext>(options =>
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
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    context.Database.EnsureCreated();
}

// CRUD Endpoints
app.MapGet("/users", async (UserDbContext context) =>
{
    var users = await context.Users.Where(u => u.IsActive).ToListAsync();
    return Results.Ok(users);
});

app.MapGet("/users/{id}", async (int id, UserDbContext context) =>
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/users", async (User user, UserDbContext context) =>
{
    user.CreatedAt = DateTime.UtcNow;
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", async (int id, User updatedUser, UserDbContext context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();
    
    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    user.UserType = updatedUser.UserType;
    user.CompanyName = updatedUser.CompanyName;
    user.PhoneNumber = updatedUser.PhoneNumber;
    user.Address = updatedUser.Address;
    user.IsActive = updatedUser.IsActive;
    user.UpdatedAt = DateTime.UtcNow;
    
    await context.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapDelete("/users/{id}", async (int id, UserDbContext context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();
    
    user.IsActive = false;
    user.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.Run();

