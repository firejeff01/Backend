using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Backend.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend.Api", Version = "v1" });
                c.EnableAnnotations();

                // Add JWT bearer definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddSharedJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            // register shared password hasher as default for DI (bcrypt)
            services.AddSingleton<Backend.Shared.Security.IPasswordHasher, Backend.Shared.Security.BcryptPasswordHasher>();

            var jwtKey = config["Jwt:Key"] ?? "ThisIsASecretKeyForDevChangeMe123!";
            var jwtIssuer = config["Jwt:Issuer"] ?? "BackendApi";
            var jwtAudience = config["Jwt:Audience"] ?? "BackendApiUsers";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
