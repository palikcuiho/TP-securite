using System.Threading.RateLimiting;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var identityServerConfigurationSection =
    builder.Configuration.GetSection("IdentityServer") as IdentityServerConfiguration;
builder.Services.Configure<IdentityServerConfiguration>(
    identityServerConfigurationSection
        ?? throw new Exception(
            message: "Identity server is not configured properly in appsettings.json"
        )
);

string reactApp =
    builder.Configuration.GetValue<string>("ClientAddress")
    ?? throw new Exception(
        message: "Client address is not configured properly in appsettings.json"
    );

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connectionString));

builder
    .Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
            identityServerConfigurationSection.LockoutDuration
        );
        options.Lockout.MaxFailedAccessAttempts =
            identityServerConfigurationSection.LockoutThreshold;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7221/";
        //options.Authority = "http://localhost:5216/";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters =
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false,
            };
    });
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
});

builder.Services.AddRateLimiter(_ =>
    _.AddFixedWindowLimiter(
        policyName: "fixedWindow",
        options =>
        {
            options.PermitLimit = 100;
            options.Window = TimeSpan.FromMinutes(10);
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 2;
        }
    )
);

builder.Services.AddScoped<IProfileService, IdentityProfileService>();
builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        builder =>
        {
            builder.WithOrigins(reactApp).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        }
    );
});

List<ApiScope> apiScopes = [];
foreach (var scope in identityServerConfigurationSection.Scopes)
{
    apiScopes.Add(new ApiScope(scope));
}
;

builder
    .Services.AddIdentityServer(options =>
    {
        options.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<IdentityUser>()
    .AddProfileService<IdentityProfileService>()
    .AddInMemoryClients(
        [
            new Client
            {
                ClientId = identityServerConfigurationSection.ClientId,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret(
                        (
                            Environment.GetEnvironmentVariable("SECRET")
                            ?? throw new Exception("Secret missing from .env")
                        ).Sha256()
                    ),
                },
                AllowedScopes = identityServerConfigurationSection.Scopes,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 2592000,
            },
        ]
    )
    .AddInMemoryApiScopes(apiScopes)
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();
