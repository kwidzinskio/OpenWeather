using OpenWeather.DatabaseLayer.Entities;

namespace OpenWeather.Models
{
    public class OpenWeatherApp
    {
        public string Response { get; set; }
        public List<WeatherInfo> WeatherInfos { get; set; }
    }
}
