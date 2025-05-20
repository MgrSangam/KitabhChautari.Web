using KitabhChautari.Api.Data;
using KitabhChautari.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPasswordHasher<Admin>, PasswordHasher<Admin>>();
var app = builder.Build();
ApplyDbMigrations(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
static void ApplyDbMigrations(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var context = sp.GetRequiredService<KitabhChautariDBContext>();
    if (context.Database.GetPendingMigrations().Any()
        {
        context.Database.Migrate();
    }
}
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
