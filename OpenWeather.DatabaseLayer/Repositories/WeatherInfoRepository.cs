using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.DatabaseLayer.Repositories
{
    public class WeatherInfoRepository : IWeatherInfoRepository
    {
        private readonly WeatherContext context;
        public WeatherInfoRepository(WeatherContext _context)
        {
            context = _context;
        }

        public async Task AddWeatherInfo(WeatherInfo weatherInfo)
        {
            await context.WeatherInfos.AddAsync(weatherInfo);
            await context.SaveChangesAsync();
        }
    }
}
