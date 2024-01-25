using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Entities;
using System.Diagnostics;
using System.Net;
using System.Text;
using OpenWeather.BusinessLogic.Services;
using OpenWeather.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeather.Controllers
{
    public class HomeController : Controller
    {
        private OpenWeatherApp openWeatherMap = new OpenWeatherApp();
        private readonly IWeatherService weatherService;

        public HomeController(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        public ActionResult Index()
        {
            return View(openWeatherMap);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string selectedCities, string action)
        {
            if (!string.IsNullOrEmpty(selectedCities))
            {
                List<string> cities = selectedCities.Split(',').ToList();
                string weatherInfo;
                switch (action)
                {
                    case "showLast":
                        weatherInfo = await weatherService.GetCurrentWeather(cities);
                        openWeatherMap.response = weatherInfo;
                        break;
                    case "showHistory":
                        weatherInfo = await weatherService.GetHistorytWeather(cities);
                        openWeatherMap.response = weatherInfo;
                        break;
                    case "downloadLast":
                        var csv = await weatherService.ConvertCurrentWeatherToCsv(cities);

                        var byteArray = Encoding.ASCII.GetBytes(csv);
                        var stream = new MemoryStream(byteArray);

                        return File(stream, "text/csv", "weatherData.csv");
                     case "downloadHistory":
                        var csv2 = await weatherService.ConvertHistoryWeatherToCsv(cities);

                        var byteArray2 = Encoding.ASCII.GetBytes(csv2);
                        var stream2 = new MemoryStream(byteArray2);

                        return File(stream2, "text/csv", "weatherData.csv");
                }
            }
            else
            {
                 openWeatherMap.response = "Choose city first";
            }

            return View(openWeatherMap);
        }

        [HttpGet]
        public async Task<ActionResult> Dummy()
        {
            return View("Index");
        }
    }
}