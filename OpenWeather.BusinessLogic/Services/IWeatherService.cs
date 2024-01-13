using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Services
{
    public interface IWeatherService
    {
        Task<string> GetWeatherSearch(string city);
    }
}
