using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarthquakeChart.API.Models
{
    public class EarthquakeChart
    {
        public EarthquakeChart()
        {
            Counts = new List<int>();
        }

        public string EarthquakeDate { get; set; }

        public List<int> Counts { get; set; }
    }
}