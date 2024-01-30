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
        private readonly OpenWeatherApp openWeatherMap = new OpenWeatherApp();
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
                        var streamCurrent = await weatherService.ReportCurrentWeather(cities);
                        return File(streamCurrent, "text/csv", "weatherData.csv");
                     case "downloadHistory":
                        var streamHistory = await weatherService.ReportHistoryWeather(cities);
                        return File(streamHistory, "text/csv", "weatherData.csv");
                }
            }
            else
            {
                 openWeatherMap.response = "Choose city first";
            }

            return View(openWeatherMap);
        }
    }
}