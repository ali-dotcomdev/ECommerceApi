using ECommerceApi.Application.Interfaces.Repositories;
using ECommerceApi.Application.Interfaces.Services;
using ECommerceApi.Application.Services;
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

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(); // Otomatik kontrolü açar
builder.Services.AddFluentValidationClientsideAdapters(); // İstemci tarafı desteği (opsiyonel)
builder.Services.AddValidatorsFromAssemblyContaining<ECommerceApi.Application.Services.AuthService>();
// NOT: Yukarıdaki satırda 'AuthService' yerine Application katmanındaki herhangi bir sınıfı verebilirsiniz.
// Bu komut, o katmandaki tüm Validator sınıflarını (UserLoginDtoValidator vs.) bulur ve kaydeder.

builder.Services.AddOpenApi();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
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
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ECommerceApi.API.Middlewares.GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();