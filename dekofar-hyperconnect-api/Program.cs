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
using Dekofar.HyperConnect.Infrastructure.ServiceRegistration;
using Dekofar.HyperConnect.Integrations.NetGsm.Interfaces;
using Dekofar.HyperConnect.Integrations.NetGsm.Services;
using Dekofar.HyperConnect.Integrations.Shopify.Interfaces;
using Dekofar.HyperConnect.Integrations.Shopify.Services;

var builder = WebApplication.CreateBuilder(args);

// 🌐 CORS
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

// 📦 Altyapı servisleri (Identity, DbContext, Application servisleri, ...)
builder.Services.AddInfrastructure(builder.Configuration);


// 📨 NetGSM & Shopify servisleri
builder.Services.AddScoped<INetGsmSmsService, NetGsmSmsService>();
builder.Services.AddHttpClient<IShopifyService, ShopifyService>();

// 📡 JSON ve Controller
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

// 📘 Swagger + JWT
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
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

// 🧪 Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dekofar API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication(); // JWT buradan aktif olur
app.UseAuthorization();
app.MapControllers();

// 🚀 Roller otomatik oluşturulsun
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
