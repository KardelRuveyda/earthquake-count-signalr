using EarthquakeChart.API.Hubs;
using EarthquakeChart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EarthquakeChart.API.Services
{
    public class EarthquakeService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<EarthquakeHub> _hubContext;

        public EarthquakeService(AppDbContext context, IHubContext<EarthquakeHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Earthquake> GetList()
        {
            return _context.Earthquakes.AsQueryable();
        }

        public async Task SaveEartquake(Earthquake e)
        {
            await _context.Earthquakes.AddAsync(e);
            await _context.SaveChangesAsync();

            // her bir data kaydedildiğinde ( her bir data kaydedildiğinde ) data gönderiliyor
            await _hubContext.Clients.All.SendAsync("ReceiveEarthquake", GetEarthquakeChartList());
        }

        public List<EarthquakeChart.API.Models.EarthquakeChart> GetEarthquakeChartList()

        {
            List<EarthquakeChart.API.Models.EarthquakeChart> covidCharts = new List<EarthquakeChart.API.Models.EarthquakeChart>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih,[1],[2],[3],[4],[5]  FROM(select[City],[Count], Cast([EarthquakeDate] as date) as tarih FROM Earthquakes) as earthquakeT PIVOT (SUM(Count) For City IN([1],[2],[3],[4],[5]) ) as ptable order by tarih asc";

                command.CommandType = System.Data.CommandType.Text;

                _context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EarthquakeChart.API.Models.EarthquakeChart ec = new EarthquakeChart.API.Models.EarthquakeChart();

                        ec.EarthquakeDate = reader.GetDateTime(0).ToShortDateString();

                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                ec.Counts.Add(0);
                            }
                            else
                            {
                                ec.Counts.Add(reader.GetInt32(x));
                            }
                        });

                        covidCharts.Add(ec);
                    }
                }

                _context.Database.CloseConnection();

                return covidCharts;
            }
        }
    }
}
