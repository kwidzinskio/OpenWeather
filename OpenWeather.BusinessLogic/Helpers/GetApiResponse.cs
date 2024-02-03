using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.BusinessLogic.Services;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.DatabaseLayer.Repositories;

namespace OpenWeather.BusinessLogic.Helpers
{
    public class GetApiResponse
    {
        private readonly IWeatherInfoRepository _repository;
        public GetApiResponse(IWeatherInfoRepository repository)
        {
            _repository = repository;
        }
        public async Task<WeatherInfo> GetResponseAsync(
            HttpClient httpClient, 
            string urlOpenWeatherMap, 
            string urlAirly,
            string apiKeyAirly,
            string city)
        {
            try
            {
                var responseOpenWeatherMap = await httpClient.GetAsync(urlOpenWeatherMap);
                responseOpenWeatherMap.EnsureSuccessStatusCode();
                var apiResponseOpenWeatherMap = await responseOpenWeatherMap.Content.ReadAsStringAsync();
                ResponseWeather rootObjectOpenWeatherMap = JsonConvert.DeserializeObject<ResponseWeather>(apiResponseOpenWeatherMap);
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime normalDateTime = epoch.AddSeconds(rootObjectOpenWeatherMap.Dt);

                DateTime? lastFetchTime = await _repository.GetLastDataTimeAsync(city); 
                DateTime currentTime = DateTime.UtcNow;

                ResponsePollution rootObjectAirly;

                if (!lastFetchTime.HasValue || (currentTime - lastFetchTime.Value).TotalHours > 1)
                {
                    httpClient.DefaultRequestHeaders.Add("apikey", apiKeyAirly);
                    var responseAirly = await httpClient.GetAsync(urlAirly);
                    responseAirly.EnsureSuccessStatusCode();
                    string apiResponseAirly = await responseAirly.Content.ReadAsStringAsync();
                    httpClient.DefaultRequestHeaders.Remove("apikey");
                    rootObjectAirly = JsonConvert.DeserializeObject<ResponsePollution>(apiResponseAirly);
                }
                else
                {
                    var lastCityRecord = await _repository.GetLastAirlyDataAsync(city);
                    rootObjectAirly = new ResponsePollution
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

                var weatherInfo = new WeatherInfo
                {
                    Name = rootObjectOpenWeatherMap.Name,
                    Country = rootObjectOpenWeatherMap.Sys.Country,
                    Dt = normalDateTime,
                    Temp = rootObjectOpenWeatherMap.Main.Temp,
                    TempFeelsLike = rootObjectOpenWeatherMap.Main.TempFeelsLike,
                    Descrpition = rootObjectOpenWeatherMap.Weather[0].Description,
                    WindSpeed = rootObjectOpenWeatherMap.Wind.Speed,
                    PM25 = rootObjectAirly.Current.Values[0].PM25Value,
                    PollutionLevel = rootObjectAirly.Current.Indexes[0].Level,
                    PollutionDescription = rootObjectAirly.Current.Indexes[0].Description,
                    PollutionDescriptionColor = rootObjectAirly.Current.Indexes[0].Color,
                    Humidity = rootObjectOpenWeatherMap.Main.Humidity,
                    Pressure = rootObjectOpenWeatherMap.Main.Pressure,
                    Visibility = rootObjectOpenWeatherMap.Visibility,
                    Icon = rootObjectOpenWeatherMap.Weather[0].Icon
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
    }
}