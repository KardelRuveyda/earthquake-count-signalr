using EarthquakeChart.API.Hubs;
using EarthquakeChart.API.Models;
using EarthquakeChart.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:44305").AllowAnyHeader()
        .AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddScoped<EarthquakeService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseWebSockets();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");


// global cors policy
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<EarthquakeHub>("/EarthquakeHub");
});

app.Run();
