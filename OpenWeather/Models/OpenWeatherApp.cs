using OpenWeather.DatabaseLayer.Entities;

namespace OpenWeather.Models
{
    public class OpenWeatherApp
    {
        public string response { get; set; }

        public List<WeatherInfo> weatherInfos { get; set; }

        public string city
        {
            get; set;
        }
    }
}
