using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OpenWeather.DatabaseLayer.Entities
{
    public class WeatherInfo
    {
        [Key]
        public int Id { get; set; }
        public int Visibility { get; set; }
        public DateTime Dt { get; set; }
        public int IdApi { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Descrpition { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double TempFeelsLike { get; set; }
        public double Temp { get; set; }
        public string Icon { get; set; }
    }
}
