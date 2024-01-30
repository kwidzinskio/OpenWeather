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

        public async Task<Tuple<ResponseWeather, WeatherInfo>> GetResponseAsync(string url)
        {
            try
            {
                var response = await this.httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadAsStringAsync();

                ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                var weatherInfo = new WeatherInfo
                {
                    Visibility = rootObject.visibility,
                    Dt = rootObject.dt,
                    IdApi = rootObject.id,
                    Name = rootObject.name,
                    Country = rootObject.sys.country,
                    Descrpition = rootObject.weather[0].description,
                    Humidity = rootObject.main.humidity,
                    WindSpeed = rootObject.wind.speed,
                    TempFeelsLike = rootObject.main.feels_like,
                    Temp = rootObject.main.temp,
                    Icon = rootObject.weather[0].icon
                };

                return Tuple.Create(rootObject, weatherInfo);
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