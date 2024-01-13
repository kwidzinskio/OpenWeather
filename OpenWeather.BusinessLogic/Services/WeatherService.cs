using OpenWeather.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenWeather.BusinessLogic.Models;

namespace OpenWeather.BusinessLogic.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<string> GetWeatherSearch(string city)
        {
            string apiKey = "4032517b4f77594f74b3d1f923b2df36";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

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
            await context.WeatherInfos.AddAsync(weatherInfo);
            await context.SaveChangesAsync();

            StringBuilder sb = new StringBuilder();
            sb.Append("<table><tr><th>Weather Description</th></tr>");
            sb.Append("<tr><td>City:</td><td>" + "Paris" + "</td></tr>");
            sb.Append("<tr><td>Country:</td><td>" + "France" + "</td></tr>");

            return sb.ToString();
        }
    }
}
