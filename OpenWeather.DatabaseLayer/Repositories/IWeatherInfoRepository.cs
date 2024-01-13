using OpenWeather.DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.DatabaseLayer.Repositories
{
    public interface IWeatherInfoRepository
    {
        Task AddWeatherInfo(WeatherInfo weatherInfo);
    }
}
