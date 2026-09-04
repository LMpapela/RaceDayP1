using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;

// Beginning of application setup
var builder = WebApplication.CreateBuilder(args);

// Register controller services
builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the RaceDay database context
builder.Services.AddDbContext<RaceDayDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RaceDayConnection")));

var app = builder.Build();

// Beginning of Swagger configuration
app.UseSwagger();
app.UseSwaggerUI();
// End of Swagger configuration

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Configure authorization
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.Run();

// End of application setup