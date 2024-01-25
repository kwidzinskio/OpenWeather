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
            List<string> cities = selectedCities.Split(',').ToList();
            if (cities != null)
            {
                string weatherInfo;
                switch (action)
                {
                    case "showLast":
                        weatherInfo = await weatherService.GetWeatherSearch(cities[0]);
                        openWeatherMap.response = weatherInfo;
                        break;
                    case "showHistory":
                        weatherInfo = await weatherService.GetWeatherSearch("Paris");
                        openWeatherMap.response = weatherInfo;
                        break;
                    case "downloadLast":
                        weatherInfo = await weatherService.GetWeatherSearch("Paris");
                        openWeatherMap.response = weatherInfo;
                        break;
                    case "downloadHistory":
                        weatherInfo = await weatherService.GetWeatherSearch("qqq");
                        openWeatherMap.response = weatherInfo;
                        break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Form["submit"].ToString()))
                {
                    openWeatherMap.response = "Input City Name";
                }
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