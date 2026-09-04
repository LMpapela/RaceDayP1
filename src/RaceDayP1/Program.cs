using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;

// Beginning of application setup
var builder = WebApplication.CreateBuilder(args);

// Register the RaceDay database context
builder.Services.AddDbContext<RaceDayDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RaceDayConnection")));

// Add controller services
builder.Services.AddControllers();

var app = builder.Build();

// Configure HTTPS redirection
app.UseHttpsRedirection();

// Configure authorization
app.UseAuthorization();

// Map API controllers
app.MapControllers();

app.Run();

// End of application setup