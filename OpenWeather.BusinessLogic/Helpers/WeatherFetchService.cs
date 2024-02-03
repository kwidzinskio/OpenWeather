using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.BusinessLogic.Services;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.DatabaseLayer.Repositories;
using System.Net.Http;

namespace OpenWeather.BusinessLogic.Helpers
{
    public class WeatherFetchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWeatherInfoRepositoryFactory _weatherInfoRepositoryFactory;
        private readonly WeatherInfoRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly string _apiKeyOpenWeatherApp;
        private readonly string _apiKeyAirly;
        public WeatherFetchService(
            IWeatherInfoRepositoryFactory weatherInfoRepositoryFactory,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory
        )
        {
            _httpClientFactory = httpClientFactory;
            _weatherInfoRepositoryFactory = weatherInfoRepositoryFactory;
            _repository = _weatherInfoRepositoryFactory.Create();
            _configuration = configuration;
            _apiKeyOpenWeatherApp = _configuration.GetSection("ApiKeyOpenWeatherApp").Value;
            _apiKeyAirly = _configuration.GetSection("ApiKeyAirly").Value;
        }

        public async Task<WeatherInfo> FetchAndCombineWeatherData(string cityName, int cityAirlyId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var weatherData = await FetchWeatherData(httpClient, cityName);
                var pollutionData = await FetchPollutionData(httpClient, cityName, cityAirlyId);

                DateTime unixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime weatherDateTime = unixEpochStart.AddSeconds(weatherData.Dt);

                var weatherInfo = new WeatherInfo
                {
                    Name = weatherData.Name,
                    Country = weatherData.Sys.Country,
                    Dt = weatherDateTime,
                    Temp = weatherData.Main.Temp,
                    TempFeelsLike = weatherData.Main.TempFeelsLike,
                    Descrpition = weatherData.Weather[0].Description,
                    WindSpeed = weatherData.Wind.Speed,
                    PM25 = pollutionData.Current.Values[0].PM25Value,
                    PollutionLevel = pollutionData.Current.Indexes[0].Level,
                    PollutionDescription = pollutionData.Current.Indexes[0].Description,
                    PollutionDescriptionColor = pollutionData.Current.Indexes[0].Color,
                    Humidity = weatherData.Main.Humidity,
                    Pressure = weatherData.Main.Pressure,
                    Visibility = weatherData.Visibility,
                    Icon = weatherData.Weather[0].Icon
                };

                return weatherInfo;
            }
            catch (WebException webEx)
            {
                Console.WriteLine($"WebException occurred: {webEx.Message}");
                throw;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JsonException occurred: {jsonEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
                throw;
            }
        }

        private async Task<ResponseWeather> FetchWeatherData(HttpClient httpClient, string cityName)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKeyOpenWeatherApp}&units=metric";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadAsStringAsync();
                var rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                return rootObject;
            }
            catch (WebException webEx)
            {
                Console.WriteLine($"WebException occurred: {webEx.Message}");
                throw;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JsonException occurred: {jsonEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
                throw;
            }
        }

        private async Task<ResponsePollution> FetchPollutionData(HttpClient httpClient, string cityName, int cityAirlyId)
        {
            DateTime? lastFetchTime = await _repository.GetLastDataTimeAsync(cityName);
            DateTime currentTime = DateTime.UtcNow;

            ResponsePollution rootObject;

            if (!lastFetchTime.HasValue || (currentTime - lastFetchTime.Value).TotalHours > 1)
            {
                string url = $"https://airapi.airly.eu/v2/measurements/installation?installationId={cityAirlyId}";

                httpClient.DefaultRequestHeaders.Add("apikey", _apiKeyAirly);
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                httpClient.DefaultRequestHeaders.Remove("apikey");
                var apiResponse = await response.Content.ReadAsStringAsync();
                rootObject = JsonConvert.DeserializeObject<ResponsePollution>(apiResponse);
            }
            else
            {
                var lastCityRecord = await _repository.GetLastAirlyDataAsync(cityName); 
                rootObject = new ResponsePollution
                {
                    Current = new PollutionData
                    {
                        Values = new List<Value>
                            {
                                new Value { PM25Value = lastCityRecord.PM25 }
                            },
                        Indexes = new List<Models.Index>
                            {
                                new Models.Index
                                {
                                    Level = lastCityRecord.PollutionLevel,
                                    Description = lastCityRecord.PollutionDescription,
                                    Color = lastCityRecord.PollutionDescriptionColor
                                }
                            }
                    }
                };
            }

            return rootObject;
        }

        public async Task AddWeatherInfo(WeatherInfo weatherInfo)
        {
            await _repository.AddWeatherInfo(weatherInfo);
        }

    }
}