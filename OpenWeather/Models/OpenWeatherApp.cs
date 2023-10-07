namespace OpenWeather.Models
{
    public class OpenWeatherApp
    {
        public string apiResponse { get; set; }

        public Dictionary<string, string> cities
        {
            get; set;
        }
    }
}
