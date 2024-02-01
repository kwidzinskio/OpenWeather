using Newtonsoft.Json;

namespace OpenWeather.BusinessLogic.Models
{
    public class OpenWeatherMapApi
    {
    }

    public class Weather
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double TempFeelsLike { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
    }

    public class ResponseWeather
    {
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public int Visibility { get; set; }
        public Wind Wind { get; set; }
        public int Dt { get; set; }
        public Sys Sys { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
