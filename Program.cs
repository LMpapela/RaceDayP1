
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaceDayP1.Data;
using RaceDayP1.Services;
using System.Security.Claims;
using System.Text;

// Beginning of application setup
var builder = WebApplication.CreateBuilder(args);

// Register controller services
builder.Services.AddControllers();

// Register application services
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<EnrolmentService>();
builder.Services.AddScoped<ResultService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<JwtService>();

// Register the RaceDay database context
var connectionString =
    builder.Configuration.GetConnectionString("RaceDayConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "The RaceDayConnection connection string is missing.");
}

builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(connectionString));

// Read and validate JWT configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key must be configured with a key of at least 32 UTF-8 bytes.");
}

if (string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "Jwt:Issuer and Jwt:Audience must be configured.");
}

// Register authentication and configure JWT validation
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ValidAlgorithms = new[]
            {
                SecurityAlgorithms.HmacSha256
            },

            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });

// Register authorization services
builder.Services.AddAuthorization();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Configure JWT Bearer authentication for Swagger
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token."
        });

    // Make the Bearer token available for Swagger API requests.
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

// Build the application
var app = builder.Build();

// Beginning of Swagger configuration
app.UseSwagger();
app.UseSwaggerUI();
// End of Swagger configuration

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Authentication must run before authorization
app.UseAuthentication();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Start the application
app.Run();

// End of application setup
