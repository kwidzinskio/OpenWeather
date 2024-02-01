using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.DatabaseLayer.Entities;

namespace OpenWeather.BusinessLogic.Helpers
{
    public class GetApiResponse
    {
        private readonly HttpClient httpClient;
        public GetApiResponse(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async static Task<WeatherInfo> GetResponseAsync(HttpClient httpClient, string urlOpenWeatherMap, string urlAirly)
        {
            try
            {
                /*                var response = await httpClient.GetAsync(urlOpenWeatherMap);
                                response.EnsureSuccessStatusCode();
                                var apiResponse = await response.Content.ReadAsStringAsync();*/

                httpClient.DefaultRequestHeaders.Add("apikey", "9ZjILXHLzBIHpBex0FTk7uMVrT7JOgNH");
                HttpResponseMessage responseAirly = await httpClient.GetAsync(urlAirly);
                responseAirly.EnsureSuccessStatusCode(); 
                string apiResponseAirly = await responseAirly.Content.ReadAsStringAsync();

                ResponseWeather rootObject = new();
                //ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime normalDateTime = epoch.AddSeconds(rootObject.Dt);

                var weatherInfo = new WeatherInfo
                {
                    Name = rootObject.Name,
                    Country = rootObject.Sys.Country,
                    Dt = normalDateTime,
                    Temp = rootObject.Main.Temp,
                    TempFeelsLike = rootObject.Main.TempFeelsLike,
                    Descrpition = rootObject.Weather[0].Description,
                    WindSpeed = rootObject.Wind.Speed,
                    Humidity = rootObject.Main.Humidity,
                    Pressure = rootObject.Main.Pressure,
                    Visibility = rootObject.Visibility,
                    Icon = rootObject.Weather[0].Icon
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