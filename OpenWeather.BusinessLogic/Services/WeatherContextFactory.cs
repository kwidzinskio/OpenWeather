using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenWeather.DatabaseLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Services
{
    public class WeatherContextFactory : IWeatherContextFactory
    {
        private readonly IConfiguration configuration;

        public WeatherContextFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public WeatherContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseContextConnectionString"));

            return new WeatherContext(optionsBuilder.Options);
        }
    }
}
