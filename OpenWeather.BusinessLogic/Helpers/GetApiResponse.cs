using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.DatabaseLayer.Repositories;

namespace OpenWeather.BusinessLogic.Helpers
{
    internal class GetApiResponse
    {
        public static Tuple<ResponseWeather, WeatherInfo> GetResponse(HttpWebRequest apiRequest) 
        {
            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

            var weatherInfo = new WeatherInfo()
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
           
    }
}
