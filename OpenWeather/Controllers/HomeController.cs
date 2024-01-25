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
        public async Task<ActionResult> Index(string city)
        {
            if (city != null)
            {
                string weatherInfo = await weatherService.GetWeatherSearch(city);
                openWeatherMap.apiResponse = weatherInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Form["submit"].ToString()))
                {
                    openWeatherMap.apiResponse = "Input City Name";
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