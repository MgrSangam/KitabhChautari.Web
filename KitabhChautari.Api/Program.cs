using KitabhChautari.Api.Data;
using KitabhChautari.Api.Data.Entities;
using KitabhChautari.Api.Endpoints;
using KitabhChautari.Api.Services;
using KitabhChautari.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddTransient<AuthService>();

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<KitabhChautariDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
    options.UseNpgsql(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
        var secretKey = builder.Configuration.GetValue<string>("Jwt:Secret");
        var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = symmetricKey,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
    {
        var allowedOriginsStr = builder.Configuration.GetValue<string>("AllowedOrigins");
        var allowedOrigins = allowedOriginsStr.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        p.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
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

app.UseCors();
app.UseAuthentication();

app.MapAuthEndpoints();

#if DEBUG
await ApplyDbMigrationsAsync(app.Services);
#endif

app.Run();

static async Task ApplyDbMigrationsAsync(IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<KitabhChautariDBContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    // Apply migrations
    if ((await context.Database.GetPendingMigrationsAsync()).Any())
    {
        await context.Database.MigrateAsync();
    }

    // Seed admin user if not exists
    if (!context.Users.Any(u => u.Email == "noel@gmail.com"))
    {
        var adminUser = new User
        {
            Id = 1,
            Name = "Noel Prince",
            Email = "noel@gmail.com",
            Phone = "9817108031",
            Role = nameof(UserRole.Admin),
            IsApproved = true,
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "9817108031");
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
    }
}