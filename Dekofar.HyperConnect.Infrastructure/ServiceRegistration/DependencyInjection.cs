using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.HyperConnect.Application.Services;
using Dekofar.HyperConnect.Infrastructure.Persistence;
using Dekofar.HyperConnect.Integrations.NetGsm.Extensions;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dekofar.HyperConnect.Infrastructure.ServiceRegistration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔌 PostgreSQL bağlantısı
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // 🔐 Identity (Guid destekli kullanıcı ve rol)
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 🔑 JWT Authentication
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // 🧾 Token servisi
            services.AddScoped<ITokenService, TokenService>();

            // ☎️ NetGSM Entegrasyonu (extension varsa)
            services.AddNetGsmIntegration(); // (isteğe bağlı)

            // 📩 NetGSM SMS Servisi
            services.AddScoped<NetGsmSmsService>();

            return services;
        }
    }
}
