namespace OpenWeather.BusinessLogic.Models
{
    public class OpenWeatherMapApi
    {
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double feels_like { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
    }

    public class Sys
    {
        public string country { get; set; }
    }

    public class ResponseWeather
    {
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
    }
}
