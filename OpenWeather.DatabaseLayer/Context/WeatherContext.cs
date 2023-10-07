using Microsoft.EntityFrameworkCore;
using OpenWeather.DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.DatabaseLayer.Context
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        public DbSet<WeatherInfo> WeatherInfos { get; set; }
    }
}
