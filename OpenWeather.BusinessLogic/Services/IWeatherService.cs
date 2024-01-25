using OpenWeather.DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Services
{
    public interface IWeatherService
    {
        Task<string> GetCurrentWeather(List<string> cities);
        Task<string> GetHistorytWeather(List<string> cities);
        Task<string> ConvertCurrentWeatherToCsv(List<string> cities);
        Task<string> ConvertHistoryWeatherToCsv(List<string> cities);
        Task GetWeatherSet();
    }
}
