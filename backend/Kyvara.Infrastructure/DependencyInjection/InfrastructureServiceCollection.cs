using System.Text;
using Kyvara.Application.Authentication.Interfaces;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Infrastructure.Authentication;
using Kyvara.Infrastructure.Identity;
using Kyvara.Infrastructure.Options;
using Kyvara.Infrastructure.Persistence;
using Kyvara.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kyvara.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        var jwt =
            configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()!;

        services.AddDbContext<KyvaraDbContext>(options =>
        {
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwt.SecretKey))
                    };
            });

        return services;
    }
}
