using EarthquakeChart.API.Models;
using EarthquakeChart.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EarthquakeChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarthquakeController : ControllerBase
    {
        private readonly EarthquakeService _service;

        public EarthquakeController(EarthquakeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SaveEarthquake(Earthquake earthquake)
        {
            await _service.SaveEartquake(earthquake);
            return Ok(_service.GetEarthquakeChartList());
        }

        [HttpGet]
        public IActionResult RandomEarthquakeCount()
        {
            Random rnd = new Random();

            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newEarthquakee = new Earthquake { City = item, Count = rnd.Next(10, 100), EarthquakeDate = DateTime.Now.AddDays(x) };
                    _service.SaveEartquake(newEarthquakee).Wait();
                    System.Threading.Thread.Sleep(1000);
                }
            });

            return Ok("Earthquake dataları veritabanına kaydedildi");
        }

    }
}
