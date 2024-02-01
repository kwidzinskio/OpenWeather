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
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly Cities _cities = new();
        private readonly IWeatherInfoRepository _weatherInfoRepository;
        private readonly IWeatherInfoRepositoryFactory _weatherInfoRepositoryFactory;
        private readonly HttpClient _httpClient;

        public WeatherService(IWeatherInfoRepository weatherInfoRepository, 
                              IConfiguration configuration, 
                              IWeatherInfoRepositoryFactory weatherInfoRepositoryFactory, 
                              HttpClient httpClient)
        {
            _weatherInfoRepository = weatherInfoRepository;
            _configuration = configuration;
            _apiKey = _configuration.GetSection("ApiKey").Value;
            _weatherInfoRepositoryFactory = weatherInfoRepositoryFactory;
            _httpClient = httpClient;
        }
        public async Task<List<WeatherInfo>> GetCurrentWeather(List<string> cities)
        {
            var repository = _weatherInfoRepositoryFactory.Create();
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
                    Pressure = rootObject.Pressure,
                    Visibility = rootObject.Visibility,
                    Icon = rootObject.Icon,
                    Dt = rootObject.Dt
                }); ;
            }

            return weatherInfos;
        }

        public async Task<List<WeatherInfo>> GetHistorytWeather(List<string> cities)
        {
            var repository = _weatherInfoRepositoryFactory.Create();
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
                        Pressure = rootObject.Pressure,
                        Visibility = rootObject.Visibility,
                        Icon = rootObject.Icon,
                        Dt = rootObject.Dt
                    });
                }
            }

            return weatherInfos;
        }

        public async Task<MemoryStream> ReportCurrentWeather(List<string> cities)
        {
            var repository = _weatherInfoRepositoryFactory.Create();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Country, City, Date, Temp, TempFeelsLike, Descriptiom, WindSpeed, Humidity, Pressure, Visibility"); ;

            foreach (var city in cities)
            {
                var info = await repository.GetCurrentWeather(city);

                stringBuilder.AppendLine($"{info.Country}, " +
                    $"{info.Name}, " +
                    $"{info.Dt}, " +
                    $"{info.Temp}, " +
                    $"{info.TempFeelsLike}, " +
                    $"{info.Descrpition}, " +
                    $"{info.WindSpeed}, " +
                    $"{info.Humidity}, " +
                    $"{info.Pressure}, " +
                    $"{info.Visibility}");
            }

            var fileContent = stringBuilder.ToString();
            var byteArray = Encoding.ASCII.GetBytes(fileContent);
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        public async Task<MemoryStream> ReportHistoryWeather(List<string> cities)
        {
            var repository = _weatherInfoRepositoryFactory.Create();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Country, City, Date, Temp, TempFeelsLike, Descriptiom, WindSpeed, Humidity, Pressure, Visibility");

            foreach (var city in cities)
            {
                var info = await repository.GetHistoryWeather(city);

                for (int i = 0; i < info.Count; i++)
                {
                    stringBuilder.AppendLine($"{info[i].Country}, " +
                        $"{info[i].Name}, " +
                        $"{info[i].Dt}, " +
                        $"{info[i].Temp}, " +
                        $"{info[i].TempFeelsLike}, " +
                        $"{info[i].Descrpition}, " +
                        $"{info[i].WindSpeed}, " +
                        $"{info[i].Humidity}, " +
                        $"{info[i].Pressure}, " +
                        $"{info[i].Visibility}");
                }
            }
            var fileContent = stringBuilder.ToString();
            var byteArray = Encoding.ASCII.GetBytes(fileContent);
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        public async Task FetchApiWeatherSet()
        {
            var repository = _weatherInfoRepositoryFactory.Create();

            foreach (var city in _cities.Read())
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
                var weatherInfo = await GetApiResponse.GetResponseAsync(_httpClient, url);

                await repository.AddWeatherInfo(weatherInfo);
            }
        }
    }
}
