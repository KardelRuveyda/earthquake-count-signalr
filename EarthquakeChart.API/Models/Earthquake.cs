namespace EarthquakeChart.API.Models
{
    public enum ECity
    {
        İstanbul = 1,
        İzmir = 2,
        Kahramanmaraş = 3,
        Adıyaman=4,
        Hatay=5
    }
    public class Earthquake
    {
        public int Id { get; set; }
        public ECity City { get; set; }
        public int Count { get; set; }
        public DateTime EarthquakeDate { get; set; }
    }
}
