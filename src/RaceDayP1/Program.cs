using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Services;

// Beginning of application setup
var builder = WebApplication.CreateBuilder(args);

// Register controller services
builder.Services.AddControllers();

// Register the Event service
builder.Services.AddScoped<EventService>();

// Register the Category service
builder.Services.AddScoped<CategoryService>();

// Register the Enrolment service
builder.Services.AddScoped<EnrolmentService>();

// Register the Result service
builder.Services.AddScoped<ResultService>();

// Register the user service
builder.Services.AddScoped<UserService>();

// Register the password hashing service
builder.Services.AddScoped<PasswordService>();

// Register the JWT service
builder.Services.AddScoped<JwtService>();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Configure JWT Bearer authentication for Swagger
    options.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter your JWT token using the format: Bearer {token}"
        });

    // Require JWT authentication for protected endpoints
    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Register the RaceDay database context
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RaceDayConnection")));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Beginning of Swagger configuration
app.UseSwagger();
app.UseSwaggerUI();
// End of Swagger configuration

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Configure authentication
app.UseAuthentication();

// Configure authorization
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.Run();

// End of application setup