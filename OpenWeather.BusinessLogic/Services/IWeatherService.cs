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
        Task<List<WeatherInfo>> GetCurrentWeather(List<string> cities);
        Task<string> GetHistorytWeather(List<string> cities);
        Task<MemoryStream> ReportCurrentWeather(List<string> cities);
        Task<MemoryStream> ReportHistoryWeather(List<string> cities);
        Task FetchApiWeatherSet();
    }
}
