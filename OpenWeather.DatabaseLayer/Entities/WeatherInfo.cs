﻿using System;
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
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Dt { get; set; }
        public double Temp { get; set; }
        public double TempFeelsLike { get; set; }
        public string Descrpition { get; set; }
        public double WindSpeed { get; set; }
        public double PM25 { get; set; }
        public string PollutionLevel { get; set; }
        public string PollutionDescription { get; set; }
        public string PollutionDescriptionColor { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public int Visibility { get; set; }
        public string Icon { get; set; }
    }
}
