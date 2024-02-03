using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using OpenWeather.BusinessLogic.Helpers;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.DatabaseLayer.Repositories;


namespace OpenWeather.BusinessLogic.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherInfoRepositoryFactory _weatherInfoRepositoryFactory;

        public WeatherService(
            IWeatherInfoRepository weatherInfoRepository, 
            IWeatherInfoRepositoryFactory weatherInfoRepositoryFactory
        )
        {
            _weatherInfoRepositoryFactory = weatherInfoRepositoryFactory;
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
                    Id = rootObject.Id,
                    Name = rootObject.Name,
                    Country = rootObject.Country,
                    Temp = rootObject.Temp,
                    TempFeelsLike = rootObject.TempFeelsLike,
                    Descrpition = rootObject.Descrpition,
                    WindSpeed = rootObject.WindSpeed,
                    PM25 = rootObject.PM25,
                    PollutionLevel = rootObject.PollutionLevel,
                    PollutionDescription = rootObject.PollutionDescription,
                    PollutionDescriptionColor = rootObject.PollutionDescriptionColor,
                    Humidity = rootObject.Humidity,
                    Pressure = rootObject.Pressure,
                    Visibility = rootObject.Visibility,
                    Icon = rootObject.Icon,
                    Dt = rootObject.Dt
                }); ;
            }

            return weatherInfos;
        }

        public async Task<List<WeatherInfo>> GetHistoryWeather(List<string> cities)
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
                        Id = rootObject.Id,
                        Name = rootObject.Name,
                        Country = rootObject.Country,
                        Temp = rootObject.Temp,
                        TempFeelsLike = rootObject.TempFeelsLike,
                        Descrpition = rootObject.Descrpition,
                        WindSpeed = rootObject.WindSpeed,
                        PM25 = rootObject.PM25,
                        PollutionLevel = rootObject.PollutionLevel,
                        PollutionDescription = rootObject.PollutionDescription,
                        PollutionDescriptionColor = rootObject.PollutionDescriptionColor,
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
            stringBuilder.AppendLine("Country, City, Date, Temp, TempFeelsLike, Descriptiom, WindSpeed, PM25, PollutionLevel, PollutionDescription, Humidity, Pressure, Visibility"); ;

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
                    $"{info.PM25}, " +
                    $"{info.PollutionLevel}, " +
                    $"{info.PollutionDescription}, " +
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
            stringBuilder.AppendLine("Country, City, Date, Temp, TempFeelsLike, Descriptiom, WindSpeed, PM25, PollutionLevel, PollutionDescription, Humidity, Pressure, Visibility");

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
                        $"{info[i].PM25}, " +
                        $"{info[i].PollutionLevel}, " +
                        $"{info[i].PollutionDescription}, " +
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
    }
}
