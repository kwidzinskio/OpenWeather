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
using Microsoft.EntityFrameworkCore;

namespace OpenWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly OpenWeatherApp openWeatherMap = new OpenWeatherApp();
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
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
                List<WeatherInfo> weatherInfos;
                switch (action)
                {
                    case "showLast":
                        weatherInfos = await _weatherService.GetCurrentWeather(cities);
                        openWeatherMap.WeatherInfos = weatherInfos;
                        break;
                    case "showHistory":
                        weatherInfos = await _weatherService.GetHistoryWeather(cities);
                        openWeatherMap.WeatherInfos = weatherInfos;
                        break;
                    case "downloadLast":
                        var streamCurrent = await _weatherService.ReportCurrentWeather(cities);
                        return File(streamCurrent, "text/csv", "weatherData.csv");
                     case "downloadHistory":
                        var streamHistory = await _weatherService.ReportHistoryWeather(cities);
                        return File(streamHistory, "text/csv", "weatherData.csv");
                }
            }
            else
            {
                 openWeatherMap.Response = "Choose city first";
            }

            return View(openWeatherMap);
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeatherInfoByCity(string city)
        {
            var cities = city.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(c => c.Trim())
                             .ToList();

            var weatherInfo = await _weatherService.GetCurrentWeather(cities);

            if (weatherInfo == null || !weatherInfo.Any())
            {
                return NotFound("Weather information for the specified citiy was not found.");
            }

            return Ok(weatherInfo[0]);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistoricalWeatherInfoByCity(string city)
        {
            var cities = city.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(c => c.Trim())
                             .ToList();

            var weatherInfos = await _weatherService.GetHistoryWeather(cities);

            if (weatherInfos == null || !weatherInfos.Any())
            {
                return NotFound("Weather information for the specified citiy was not found.");
            }

            return Ok(weatherInfos);
        }
    }
}