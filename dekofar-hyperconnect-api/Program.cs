using Dekofar.HyperConnect.Infrastructure.ServiceRegistration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🧱 Altyapı servislerini yükle (DbContext, Identity, JWT, SMS servisi vs.)
builder.Services.AddInfrastructure(builder.Configuration);

// 🔧 Controller ve Swagger ayarları
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dekofar API",
        Version = "v1"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Bearer token girin",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// ✅ Authorization (Authentication zaten AddInfrastructure'da var)
builder.Services.AddAuthorization();

// 🚀 Uygulama pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dekofar API v1");
        c.RoutePrefix = "swagger"; // tarayıcıda /swagger yazınca açılır
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
