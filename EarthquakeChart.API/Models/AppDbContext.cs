using Microsoft.EntityFrameworkCore;

namespace EarthquakeChart.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Earthquake> Earthquakes { get; set; }
    }
}
