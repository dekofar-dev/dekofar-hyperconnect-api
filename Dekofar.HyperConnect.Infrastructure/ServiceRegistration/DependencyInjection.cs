using Dekofar.HyperConnect.Application.Interfaces;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Infrastructure.Persistence;
using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Dekofar.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Dekofar.HyperConnect.Infrastructure.ServiceRegistration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 📦 PostgreSQL DbContext yapılandırması
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // 🔐 Identity yapılandırması (AppUser + Role<Guid>)
            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 🔐 JWT Authentication yapılandırması
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,

                    // 🔑 Bu satır çok kritik: [Authorize(Roles = "Admin")] için şart
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // 🎫 Uygulama Servisleri
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISupportTicketService, SupportTicketService>();
            services.AddScoped<INetGsmCallService, NetGsmCallService>();


            return services;
        }
    }
}
