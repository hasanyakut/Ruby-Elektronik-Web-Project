using Microsoft.EntityFrameworkCore;
using ServiceService.Data;
using ServiceService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework with PostgreSQL
builder.Services.AddDbContext<ServiceDbContext>(options =>
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
    var context = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    context.Database.EnsureCreated();
}

// CRUD Endpoints
app.MapGet("/servicerecords", async (ServiceDbContext context) =>
{
    var serviceRecords = await context.ServiceRecords.Where(s => s.IsActive).ToListAsync();
    return Results.Ok(serviceRecords);
});

app.MapGet("/servicerecords/all", async (ServiceDbContext context) =>
{
    var serviceRecords = await context.ServiceRecords.ToListAsync();
    return Results.Ok(serviceRecords);
});

app.MapGet("/servicerecords/{id}", async (int id, ServiceDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    return serviceRecord is not null ? Results.Ok(serviceRecord) : Results.NotFound();
});

app.MapPost("/servicerecords", async (ServiceRecord serviceRecord, ServiceDbContext context) =>
{
    serviceRecord.CreatedAt = DateTime.UtcNow;
    context.ServiceRecords.Add(serviceRecord);
    await context.SaveChangesAsync();
    return Results.Created($"/servicerecords/{serviceRecord.Id}", serviceRecord);
});

app.MapPut("/servicerecords/{id}", async (int id, ServiceRecord updatedServiceRecord, ServiceDbContext context) =>
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

app.MapDelete("/servicerecords/{id}", async (int id, ServiceDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    serviceRecord.IsActive = false;
    serviceRecord.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.NoContent();
});

app.MapPut("/servicerecords/{id}/complete", async (int id, ServiceDbContext context) =>
{
    var serviceRecord = await context.ServiceRecords.FindAsync(id);
    if (serviceRecord == null) return Results.NotFound();
    
    serviceRecord.IsActive = false;
    serviceRecord.UpdatedAt = DateTime.UtcNow;
    await context.SaveChangesAsync();
    
    return Results.Ok(serviceRecord);
});

app.Run();
