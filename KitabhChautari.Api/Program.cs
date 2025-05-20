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

builder.Services.AddDbContext<KitabhChautariDBContext>(options =>
{
    var connectingString = builder.Configuration.GetConnectionString("PostgressConnection");
    options.UseNpgsql(connectingString);

});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ApplyDbMigrations(app.Services);

app.Run();
static void ApplyDbMigrations(IServiceProvider sp)
{
    var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<KitabhChautariDBContext>();
    if (context.Database.GetPendingMigrations().Any())
        {
        context.Database.Migrate();
    }
}

