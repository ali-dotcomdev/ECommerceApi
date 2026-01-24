using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Services;
using ECommerceApi.Application.Settings;
using ECommerceApi.Infrastructure.Data;
using ECommerceApi.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var connectionString = builder.Configuration.GetConnectionString("PostgreConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddScoped(typeof(IRefreshTokenRepository), typeof(RefreshTokenRepository));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(); // Otomatik kontrolü açar
builder.Services.AddFluentValidationClientsideAdapters(); // İstemci tarafı desteği (opsiyonel)
builder.Services.AddValidatorsFromAssemblyContaining<ECommerceApi.Application.Services.AuthService>();
// NOT: Yukarıdaki satırda 'AuthService' yerine Application katmanındaki herhangi bir sınıfı verebilirsiniz.
// Bu komut, o katmandaki tüm Validator sınıflarını (UserLoginDtoValidator vs.) bulur ve kaydeder.

builder.Services.AddOpenApi();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();

var secretKey = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => // token sahte mi degil mi kontrol
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ECommerceApi.API.Middlewares.GlobalExceptionMiddleware>();
app.UseMiddleware<ECommerceApi.API.Middlewares.PerformanceMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();