using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenWeather.Class;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Entities;
using OpenWeather.Models;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace OpenWeather.Controllers
{
    public class HomeController : Controller
    {
        private WeatherContext context;
        private OpenWeatherApp openWeatherMap = new OpenWeatherApp();

        public HomeController(WeatherContext context)
        {
            this.context = context;
        }
        public ActionResult Index()
        {
            return View(openWeatherMap);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string city)
        {
            if (city != null)
            {
                string apiKey = "4032517b4f77594f74b3d1f923b2df36";
                HttpWebRequest apiRequest = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

                openWeatherMap.city = city;
                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }



                ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                var weatherInfo = new WeatherInfo()
                {
                    Base = rootObject.@base,
                    Visibility = rootObject.visibility,
                    Dt = rootObject.dt,
                    IdApi = rootObject.id,
                    Name = rootObject.name,
                    Cod = rootObject.cod,
                };
                await context.WeatherInfos.AddAsync(weatherInfo);
                await context.SaveChangesAsync();

                StringBuilder sb = new StringBuilder();
                sb.Append("<table><tr><th>Weather Description</th></tr>");
                sb.Append("<tr><td>City:</td><td>" + rootObject.name + "</td></tr>");
                sb.Append("<tr><td>Country:</td><td>" + rootObject.sys.country + "</td></tr>");
                sb.Append("<tr><td>Wind:</td><td>" + rootObject.wind.speed + " Km/h</td></tr>");
                sb.Append("<tr><td>Current Temperature:</td><td>" + rootObject.main.temp + " °C</td></tr>");
                sb.Append("<tr><td>Humidity:</td><td>" + rootObject.main.humidity + "</td></tr>");
                sb.Append("<tr><td>Weather:</td><td>" + rootObject.weather[0].description + "</td></tr>");
                sb.Append("</table>");
                openWeatherMap.apiResponse = sb.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Form["submit"].ToString()))
                {
                    openWeatherMap.apiResponse = "► Select City";
                }
            }
            return View(openWeatherMap);
        }
    }
}