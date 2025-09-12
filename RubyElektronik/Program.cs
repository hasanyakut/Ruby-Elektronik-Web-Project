using Microsoft.EntityFrameworkCore;
using RubyElektronik.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Swagger/OpenAPI
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
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseSession();

app.UseAuthorization();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// API Endpoints
app.MapGet("/api/products", async (ApplicationDbContext context) =>
{
    var products = await context.Products.Where(p => p.IsActive).ToListAsync();
    return Results.Ok(products);
});

app.MapGet("/api/products/{id}", async (int id, ApplicationDbContext context) =>
{
    var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", async (RubyElektronik.Models.Product product, ApplicationDbContext context) =>
{
    product.CreatedAt = DateTime.UtcNow;
    context.Products.Add(product);
    await context.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapPut("/api/products/{id}", async (int id, RubyElektronik.Models.Product updatedProduct, ApplicationDbContext context) =>
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

app.MapDelete("/api/products/{id}", async (int id, ApplicationDbContext context) =>
{
    var product = await context.Products.FindAsync(id);
    if (product == null) return Results.NotFound();
    
    product.IsActive = false;
    product.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

// Users API
app.MapGet("/api/users", async (ApplicationDbContext context) =>
{
    var users = await context.Users.Where(u => u.IsActive).ToListAsync();
    return Results.Ok(users);
});

app.MapGet("/api/users/{id}", async (int id, ApplicationDbContext context) =>
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.MapPost("/api/users", async (RubyElektronik.Models.User user, ApplicationDbContext context) =>
{
    user.CreatedAt = DateTime.UtcNow;
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
});

app.MapPut("/api/users/{id}", async (int id, RubyElektronik.Models.User updatedUser, ApplicationDbContext context) =>
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

app.MapDelete("/api/users/{id}", async (int id, ApplicationDbContext context) =>
{
    var user = await context.Users.FindAsync(id);
    if (user == null) return Results.NotFound();
    
    user.IsActive = false;
    user.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

// Orders API
app.MapGet("/api/orders", async (ApplicationDbContext context) =>
{
    var orders = await context.Orders.ToListAsync();
    return Results.Ok(orders);
});

app.MapGet("/api/orders/{id}", async (int id, ApplicationDbContext context) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapPost("/api/orders", async (RubyElektronik.Models.Order order, ApplicationDbContext context) =>
{
    order.CreatedAt = DateTime.UtcNow;
    order.TotalPrice = order.UnitPrice * order.Quantity;
    context.Orders.Add(order);
    await context.SaveChangesAsync();
    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapPut("/api/orders/{id}", async (int id, RubyElektronik.Models.Order updatedOrder, ApplicationDbContext context) =>
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

app.MapDelete("/api/orders/{id}", async (int id, ApplicationDbContext context) =>
{
    var order = await context.Orders.FindAsync(id);
    if (order == null) return Results.NotFound();
    
    context.Orders.Remove(order);
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

// Service Records API
app.MapGet("/api/servicerecords", async (ApplicationDbContext context) =>
{
    var serviceRecords = await context.ServiceRecords.Where(s => s.IsActive).ToListAsync();
    return Results.Ok(serviceRecords);
});

app.MapGet("/api/servicerecords/all", async (ApplicationDbContext context) =>
{
    var serviceRecords = await context.ServiceRecords.ToListAsync();
    return Results.Ok(serviceRecords);
});

app.MapGet("/api/servicerecords/{id}", async (int id, ApplicationDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    return serviceRecord is not null ? Results.Ok(serviceRecord) : Results.NotFound();
});

app.MapPost("/api/servicerecords", async (RubyElektronik.Models.ServiceRecord serviceRecord, ApplicationDbContext context) =>
{
    serviceRecord.CreatedAt = DateTime.UtcNow;
    context.ServiceRecords.Add(serviceRecord);
    await context.SaveChangesAsync();
    return Results.Created($"/api/servicerecords/{serviceRecord.Id}", serviceRecord);
});

app.MapPut("/api/servicerecords/{id}", async (int id, RubyElektronik.Models.ServiceRecord updatedServiceRecord, ApplicationDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    serviceRecord.Ad = updatedServiceRecord.Ad;
    serviceRecord.Soyad = updatedServiceRecord.Soyad;
    serviceRecord.UserType = updatedServiceRecord.UserType;
    serviceRecord.FirmaAdi = updatedServiceRecord.FirmaAdi;
    serviceRecord.TelefonNumarasi = updatedServiceRecord.TelefonNumarasi;
    serviceRecord.UrunTuru = updatedServiceRecord.UrunTuru;
    serviceRecord.ArizaAciklamasi = updatedServiceRecord.ArizaAciklamasi;
    serviceRecord.IsActive = updatedServiceRecord.IsActive;
    serviceRecord.UpdatedAt = DateTime.UtcNow;
    
    await context.SaveChangesAsync();
    return Results.Ok(serviceRecord);
});

app.MapDelete("/api/servicerecords/{id}", async (int id, ApplicationDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    serviceRecord.IsActive = false;
    serviceRecord.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.MapDelete("/api/servicerecords/{id}/permanent", async (int id, ApplicationDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    context.ServiceRecords.Remove(serviceRecord);
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.MapPut("/api/servicerecords/{id}/complete", async (int id, ApplicationDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    serviceRecord.IsActive = false;
    serviceRecord.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.Ok(serviceRecord);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
