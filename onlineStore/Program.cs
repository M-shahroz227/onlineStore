using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using onlineStore.Authorization;
using onlineStore.Data;
using onlineStore.Data.Seed;
using onlineStore.Service.AuthService;
using onlineStore.Service.Implementations;
using onlineStore.Service.Interfaces;
using onlineStore.Service.ProductService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// DATABASE
// =========================
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// SERVICES (DI)
// =========================
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IUserFeatureService, UserFeatureService>();
builder.Services.AddScoped<IUserService, UserService>();

// =========================
// AUTHORIZATION (FEATURE BASED)
// =========================
builder.Services.AddScoped<IAuthorizationHandler, FeatureAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, FeaturePolicyProvider>();

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<StoreDbContext>();

    FeatureSeeder.Seed(context);
}

// no hardcoded policies here
// ✔ FeatureAuthorizeAttribute + FeatureAuthorizationHandler handle everything


// =========================
// JWT AUTHENTICATION
// =========================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsASecretKeyForJwtTokenGeneration")),
        ClockSkew = TimeSpan.Zero
    };
});

// =========================
// MVC + SWAGGER
// =========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// HTTP PIPELINE
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚠️ ORDER IS VERY IMPORTANT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
