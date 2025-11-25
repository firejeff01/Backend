using Backend.Domain.Interfaces;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Domain.Services;
using Backend.Domain.Entities;
using Backend.Api.Utils;
using Microsoft.Extensions.Configuration;
using Backend.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Use shared extension to configure Swagger
builder.Services.AddSharedSwagger();

// Use shared extension to configure JWT authentication (registers IPasswordHasher too)
builder.Services.AddSharedJwtAuthentication(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "server=localhost;port=3306;database=Backend;user=root;password=;";
builder.Services.AddDbContext<BackendDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserRepository, Backend.Infrastructure.Repositories.UserRepository>();

// Register AuthService with factory to inject JWT settings
builder.Services.AddScoped<IAuthService>(sp =>
{
    var repo = sp.GetRequiredService<IUserRepository>();
    var cfg = sp.GetRequiredService<IConfiguration>();
    var key = cfg["Jwt:Key"] ?? "ThisIsASecretKeyForDevChangeMe123!";
    var issuer = cfg["Jwt:Issuer"];
    var audience = cfg["Jwt:Audience"];
    var hasher = sp.GetRequiredService<Backend.Shared.Security.IPasswordHasher>();
    return new AuthService(repo, hasher, key, issuer, audience);
});

// Register application layer AuthAppService
builder.Services.AddScoped<Backend.Api.ApplicationServices.Interfaces.IAuthAppService, Backend.Api.ApplicationServices.AuthAppService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BackendDbContext>();
    db.Database.EnsureCreated();

    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var existing = await userRepo.GetByEmailAsync("firejeff01@hotmail.com");
    if (existing == null)
    {
        var hasher = scope.ServiceProvider.GetRequiredService<Backend.Shared.Security.IPasswordHasher>();
        var user = new Backend.Domain.Entities.User
        {
            Account = "admin",
            Name = "admin",
            Email = "firejeff01@hotmail.com",
            Password = hasher.Hash("Aa12345678"),
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow
        };
        await userRepo.AddAsync(user);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Serve Swagger UI at application root ("/") in development for convenience
    app.UseSwaggerUI(c => {
        c.RoutePrefix = string.Empty;
        // Ensure Swagger UI loads the generated JSON from the correct endpoint
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();