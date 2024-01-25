using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IWeatherInfoRepository weatherInfoRepository;
        private readonly IConfiguration configuration;
        private readonly string apiKey;
        private readonly IWeatherInfoRepositoryFactory weatherInfoRepositoryFactory;
        public WeatherService(IWeatherInfoRepository _weatherInfoRepository, IConfiguration _configuration, IWeatherInfoRepositoryFactory _weatherInfoRepositoryFactory)
        {
            weatherInfoRepository = _weatherInfoRepository;
            configuration = _configuration;
            apiKey = configuration.GetSection("ApiKey").Value;
            weatherInfoRepositoryFactory = _weatherInfoRepositoryFactory;
        }
        public async Task<string> GetWeatherSearch(string city)
        {
            try
            {
                HttpWebRequest apiRequest = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

                var (rootObject, weatherInfo) = GetApiResponse.GetResponse(apiRequest);

                await weatherInfoRepository.AddWeatherInfo(weatherInfo);

                StringBuilder sb = new StringBuilder();
                sb.Append("<table><tr><th>Weather Description</th></tr>");
                sb.Append("<tr><td>City:</td><td>" + rootObject.name + "</td></tr>");
                sb.Append("<tr><td>Country:</td><td>" + rootObject.sys.country + "</td></tr>");
                sb.Append("<tr><td>Current Temperature:</td><td>" + rootObject.main.temp + " °C</td></tr>");
                sb.Append("<tr><td>Feelslike Temperature:</td><td>" + rootObject.main.feels_like + " °C</td></tr>");
                sb.Append("<tr><td>Weather:</td><td>" + rootObject.weather[0].description + "</td></tr>");
                sb.Append("<tr><td>Wind:</td><td>" + rootObject.wind.speed + " Km/h</td></tr>");
                sb.Append("<tr><td>Pressure:</td><td>" + rootObject.main.pressure + " °C</td></tr>");
                sb.Append("<tr><td>Humidity:</td><td>" + rootObject.main.humidity + "</td></tr>");
                sb.Append("<tr><td>Icon:</td><td>" + rootObject.weather[0].icon + "</td></tr>");
                sb.Append("</table>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "No data";
            }
        }

        public async Task GetWeatherSet()
        {
            Cities cities = new Cities();

            foreach (var city in cities.ReadonlyCities)
            {
                var xxx = weatherInfoRepositoryFactory.Create();
                
                    HttpWebRequest apiRequest = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

                    var (rootObject, weatherInfo) = GetApiResponse.GetResponse(apiRequest);

                    await xxx.AddWeatherInfo(weatherInfo);
                    //await dbContext.SaveChangesAsync();
                
            }
        }
    }
}
