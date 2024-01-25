using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Services
{
    public class WeatherInfoRepositoryFactory : IWeatherInfoRepositoryFactory
    {
        private readonly IConfiguration configuration;

        public WeatherInfoRepositoryFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public WeatherInfoRepository Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeatherContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseContextConnectionString"));

            var context = new WeatherContext(optionsBuilder.Options);
            return new WeatherInfoRepository(context);
        }
    }
}
