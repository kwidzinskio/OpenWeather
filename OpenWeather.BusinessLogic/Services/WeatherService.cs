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
        public async Task<string> GetCurrentWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            StringBuilder sb = new StringBuilder();

            foreach (var city in cities)
            {
                var rootObject = await repository.GetCurrentWeather(city);

                sb.Append("<tr>");
                sb.Append("<table><tr><th>Weather Description</th></tr>");
                sb.Append("<tr><td>City:</td><td>" + rootObject.Name + "</td></tr>");
                sb.Append("<tr><td>Country:</td><td>" + rootObject.Country + "</td></tr>");
                sb.Append("<tr><td>Current Temperature:</td><td>" + rootObject.Temp + " °C</td></tr>");
                sb.Append("<tr><td>Feelslike Temperature:</td><td>" + rootObject.TempFeelsLike + " °C</td></tr>");
                sb.Append("<tr><td>Weather:</td><td>" + rootObject.Descrpition + "</td></tr>");
                sb.Append("<tr><td>Wind:</td><td>" + rootObject.WindSpeed + " Km/h</td></tr>");
                sb.Append("<tr><td>Humidity:</td><td>" + rootObject.Humidity + "</td></tr>");
                sb.Append("<tr><td>Icon:</td><td>" + rootObject.Icon + "</td></tr>");
                sb.Append("</tr></table>");
            }

            return sb.ToString();
        }

        public async Task<string> GetHistorytWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            StringBuilder sb = new StringBuilder();

            foreach (var city in cities)
            {
                var rootObject = await repository.GetLimitedHistoryWeather(city);

                for (int i = 0; i < rootObject.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.Append("<table><tr><th>Weather Description</th></tr>");
                    sb.Append("<tr><td>City:</td><td>" + rootObject[i].Name + "</td></tr>");
                    sb.Append("<tr><td>Country:</td><td>" + rootObject[i].Country + "</td></tr>");
                    sb.Append("<tr><td>Current Temperature:</td><td>" + rootObject[i].Temp + " °C</td></tr>");
                    sb.Append("<tr><td>Feelslike Temperature:</td><td>" + rootObject[i].TempFeelsLike + " °C</td></tr>");
                    sb.Append("<tr><td>Weather:</td><td>" + rootObject[i].Descrpition + "</td></tr>");
                    sb.Append("<tr><td>Wind:</td><td>" + rootObject[i].WindSpeed + " Km/h</td></tr>");
                    sb.Append("<tr><td>Humidity:</td><td>" + rootObject[i].Humidity + "</td></tr>");
                    sb.Append("<tr><td>Icon:</td><td>" + rootObject[i].Icon + "</td></tr>");
                    sb.Append("</tr></table>");
                }
            }

            return sb.ToString();
        }

        public async Task<MemoryStream> ReportCurrentWeather(List<string> cities)
        {
            var repository = weatherInfoRepositoryFactory.Create();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Country, City, Temperature, TemperatureFeelsLike, Descriptiom, WindSpeed, Humidity, Date");

            foreach (var city in cities)
            {
                var info = await repository.GetCurrentWeather(city);

                stringBuilder.AppendLine($"{info.Country}, {info.Name}, {info.Temp}, {info.TempFeelsLike}, {info.Descrpition}, {info.WindSpeed}, {info.Humidity}, {info.Dt}");
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
            stringBuilder.AppendLine("Country, City, Temperature, TemperatureFeelsLike, Descriptiom, WindSpeed, Humidity, Date");

            foreach (var city in cities)
            {
                var info = await repository.GetHistoryWeather(city);

                for (int i = 0; i < info.Count; i++)
                {
                    stringBuilder.AppendLine($"{info[i].Country}, {info[i].Name}, {info[i].Temp}, {info[i].TempFeelsLike}, {info[i].Descrpition}, {info[i].WindSpeed}, {info[i].Humidity}, {info[i].Dt}");
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
