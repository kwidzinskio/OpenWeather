using Microsoft.Extensions.Logging;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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

        public async Task<WeatherInfo> GetCurrentWeather(string city)
        {
            try
            {
                var weatherInfo = await context.WeatherInfos
                               .Where(e => e.Name.Equals(city))
                               .OrderByDescending(e => e.Dt)
                               .FirstOrDefaultAsync();

                return weatherInfo;
            }

            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<WeatherInfo>> GetLimitedHistoryWeather(string city)
        {
            try
            {
                string sqlQuery = "SELECT TOP 5 * FROM WeatherInfos WHERE Name = @city ORDER BY NEWID()";

                var weatherInfos = await context.WeatherInfos
                                                .FromSqlRaw(sqlQuery, new SqlParameter("@city", city))
                                                .ToListAsync();

                return weatherInfos;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WeatherInfo>> GetHistoryWeather(string city)
        {
            try
            {
                var weatherInfos = await context.WeatherInfos
                               .Where(e => e.Name.Equals(city))
                               .ToListAsync();

                return weatherInfos;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
