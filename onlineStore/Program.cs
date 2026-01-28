using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using onlineStore.Authorization;
using onlineStore.Common;
using onlineStore.Data;
using onlineStore.Data.Seed;
using onlineStore.Service.AuthService;
using onlineStore.Service.Implementations;
using onlineStore.Service.Interfaces;
using onlineStore.Service.ProductService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// CONFIGURATION
// =========================
var jwtSecret = builder.Configuration.GetValue<string>("JwtSettings:Secret")
                ?? "ThisIsASecretKeyForJwtTokenGeneration";

// =========================
// DATABASE
// =========================
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// DEPENDENCY INJECTION
// =========================
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IUserFeatureService, UserFeatureService>();
builder.Services.AddScoped<IUserService, UserService>();

// =========================
// FEATURE-BASED AUTHORIZATION
// =========================
builder.Services.AddScoped<IAuthorizationHandler, FeatureAuthorizationHandler>();

// Dynamically register feature policies from AppFeatures
builder.Services.AddAuthorization(options =>
{
    var featureNames = new[]
    {
        AppFeatures.PRODUCT_VIEW,
        AppFeatures.PRODUCT_CREATE,
        AppFeatures.PRODUCT_DELETE
    };

    foreach (var feature in featureNames)
    {
        options.AddPolicy($"Feature.{feature}", policy =>
            policy.Requirements.Add(new FeatureRequirement(feature)));
    }
});

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
        ValidateIssuer = false, // set true in production
        ValidateAudience = false, // set true in production
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

// =========================
// CONTROLLERS + SWAGGER
// =========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Bearer support in Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// =========================
// BUILD APP
// =========================
var app = builder.Build();

// =========================
// SEED FEATURES
// =========================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    FeatureSeeder.Seed(context);
}

// =========================
// HTTP PIPELINE
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ⚠️ Must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
