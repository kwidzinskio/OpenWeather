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
        public string Base { get; set; }
        public int Visibility { get; set; }
        public int Dt { get; set; }
        public int IdApi { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }
    }
}
