using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Helpers;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.DatabaseLayer.Repositories;


namespace OpenWeather.BusinessLogic.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration configuration;
        private readonly string apiKey;
        private readonly Cities cities = new();
        private readonly IWeatherInfoRepository weatherInfoRepository;
        private readonly IWeatherInfoRepositoryFactory weatherInfoRepositoryFactory;
        private readonly HttpClient httpClient;

        public WeatherService(IWeatherInfoRepository _weatherInfoRepository, 
                              IConfiguration _configuration, 
                              IWeatherInfoRepositoryFactory _weatherInfoRepositoryFactory, 
                              HttpClient _httpClient)
        {
            weatherInfoRepository = _weatherInfoRepository;
            configuration = _configuration;
            apiKey = configuration.GetSection("ApiKey").Value;
            weatherInfoRepositoryFactory = _weatherInfoRepositoryFactory;
            httpClient = _httpClient;
        }
        public async Task<List<WeatherInfo>> GetCurrentWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            List<WeatherInfo> weatherInfos = new List<WeatherInfo>();

            foreach (var city in cities)
            {
                var rootObject = await repository.GetCurrentWeather(city);
                weatherInfos.Add(new WeatherInfo
                {
                    Name = rootObject.Name,
                    Country = rootObject.Country,
                    Temp = rootObject.Temp,
                    TempFeelsLike = rootObject.TempFeelsLike,
                    Descrpition = rootObject.Descrpition,
                    WindSpeed = rootObject.WindSpeed,
                    Humidity = rootObject.Humidity,
                    Icon = rootObject.Icon,
                    Dt = rootObject.Dt
                });
            }

            return weatherInfos;
        }

        public async Task<List<WeatherInfo>> GetHistorytWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            List<WeatherInfo> weatherInfos = new List<WeatherInfo>();

            foreach (var city in cities)
            {
                var weatherInfosDb = await repository.GetLimitedHistoryWeather(city);
                foreach (var rootObject in weatherInfosDb)
                {
                    weatherInfos.Add(new WeatherInfo
                    {
                        Name = rootObject.Name,
                        Country = rootObject.Country,
                        Temp = rootObject.Temp,
                        TempFeelsLike = rootObject.TempFeelsLike,
                        Descrpition = rootObject.Descrpition,
                        WindSpeed = rootObject.WindSpeed,
                        Humidity = rootObject.Humidity,
                        Icon = rootObject.Icon,
                        Dt = rootObject.Dt
                    });
                }
            }

            return weatherInfos;
        }

        public async Task<MemoryStream> ReportCurrentWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Country, City, Date, Temperature, TemperatureFeelsLike, Descriptiom, WindSpeed, Humidity, Date");

            foreach (var city in cities)
            {
                var info = await repository.GetCurrentWeather(city);

                stringBuilder.AppendLine($"{info.Country}, {info.Name}, {info.Dt}, {info.Temp}, {info.TempFeelsLike}, {info.Descrpition}, {info.WindSpeed}, {info.Humidity}");
            }

            var fileContent = stringBuilder.ToString();
            var byteArray = Encoding.ASCII.GetBytes(fileContent);
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        public async Task<MemoryStream> ReportHistoryWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Country, City, Date, Temperature, TemperatureFeelsLike, Descriptiom, WindSpeed, Humidity");

            foreach (var city in cities)
            {
                var info = await repository.GetHistoryWeather(city);

                for (int i = 0; i < info.Count; i++)
                {
                    stringBuilder.AppendLine($"{info[i].Country}, {info[i].Name}, {info[i].Dt}, {info[i].Temp}, {info[i].TempFeelsLike}, {info[i].Descrpition}, {info[i].WindSpeed}, {info[i].Humidity}, {info[i].Dt}");
                }
            }
            var fileContent = stringBuilder.ToString();
            var byteArray = Encoding.ASCII.GetBytes(fileContent);
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        public async Task FetchApiWeatherSet()
        {
            var repository = weatherInfoRepositoryFactory.Create();

            foreach (var city in cities.Read())
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                var (rootObject, weatherInfo) = await GetApiResponse.GetResponseAsync(httpClient, url);

                await repository.AddWeatherInfo(weatherInfo);
            }
        }
    }
}
