using EarthquakeChart.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace EarthquakeChart.API.Hubs
{
    public class EarthquakeHub:Hub
    {
        private readonly EarthquakeService _service;

        public EarthquakeHub(EarthquakeService service)
        {
            _service = service; 
        }
        public async Task GetEarthquakeList()
        {
            await Clients.All.SendAsync("ReceiveEarthquake", _service.GetEarthquakeChartList());
        }
    }
}
