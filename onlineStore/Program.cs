using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using onlineStore.Authorization;
using onlineStore.Data;
using onlineStore.Filters.LogActionFilter;
using onlineStore.Service.AuthService;
using onlineStore.Service.Implementations;
using onlineStore.Service.Interfaces;
using onlineStore.Service.ProductService;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// CONFIGURATION
// =========================
var jwtSecret = builder.Configuration.GetValue<string>("JwtSettings:Secret");
               

// =========================
// DATABASE CONTEXT
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

// Register handler
builder.Services.AddScoped<IAuthorizationHandler, FeatureAuthorizationHandler>();

// filter Registration
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<ProductResourceFilter>();
builder.Services.AddScoped<ProductResultFilter>();

// Exception filter global
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// =========================
// JWT AUTHENTICATION
// =========================


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

// =========================
// FEATURE-BASED AUTHORIZATION
// =========================


// Configure feature policies
builder.Services.AddAuthorization(options =>
{
    var featureNames = new[] { "PRODUCT_VIEW", "PRODUCT_CREATE", "PRODUCT_UPDATE", "PRODUCT_DELETE" };
    foreach (var feature in featureNames)
    {
        // Change colon to dot
        options.AddPolicy($"Feature.{feature}", policy =>
            policy.Requirements.Add(new FeatureRequirement(feature)));
    }
});




// =========================
// CONTROLLERS + SWAGGER

// =========================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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
            Array.Empty<string>()
        }
    });
});

// =========================
// BUILD APP
// =========================
var app = builder.Build();

// =========================
// SEED FEATURES (INITIAL DATA)
// =========================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    DataSeeder.Seed(context); // Features, Users, Roles, UserFeatures
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
app.UseAuthentication(); // MUST be before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
