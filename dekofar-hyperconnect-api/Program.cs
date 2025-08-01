using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Dekofar.Domain.Entities;
using Dekofar.HyperConnect.Domain.Entities;
using Dekofar.HyperConnect.Infrastructure.Persistence;
using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Dekofar.HyperConnect.Integrations.Shopify.Services;
using Dekofar.HyperConnect.Application; // Application servis kayıtları
using MediatR;
using Dekofar.HyperConnect.Infrastructure.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// 🌐 CORS Politikası
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://192.168.1.100:4200",
            "https://hyperconnect.dekofar.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// 📦 Altyapı Servisleri (DbContext, Identity, JWT vs.)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddApplication();

// 📬 Entegrasyon Servisleri
builder.Services.AddScoped<INetGsmSmsService, NetGsmSmsService>();
builder.Services.AddHttpClient<IShopifyService, ShopifyService>();

// 📡 Controller & JSON Ayarları
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

// 📘 Swagger + JWT Destekli Dokümantasyon
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dekofar API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Bearer Token için `Bearer {token}` formatında giriniz",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// 📋 Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// 🧪 Swagger Arayüzü - Her ortamda aktif
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // <- Bu çok önemli
}

// 🌐 Orta Katmanlar
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 🚀 Uygulama Başlarken Roller Oluştur
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    string[] roles = new[] { "Admin", "PERSONEL", "DEPO", "IADE", "MUSTERI_TEM" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}

app.Run();
