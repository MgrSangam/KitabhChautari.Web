using KitabhChautari.Api.Data;
using KitabhChautari.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<KitabhChautariDBContext>(options =>
{
    var connectingString = builder.Configuration.GetConnectionString("PostgresConnection");
    options.UseNpgsql(connectingString);

});
var app = builder.Build();

#if DEBUG
await ApplyDbMigrationsAsync(app.Services); // Fix: Added 'await' to ensure the method is awaited.
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

static async Task ApplyDbMigrationsAsync(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<KitabhChautariDBContext>();
    if ((await context.Database.GetPendingMigrationsAsync()).Any())
    {
        await context.Database.MigrateAsync();
    }
}
