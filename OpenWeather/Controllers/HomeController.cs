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
        //private WeatherContext context;
        private OpenWeatherApp openWeatherMap = new OpenWeatherApp();
        private readonly IWeatherService weatherService;
        private Timer timer;

        public HomeController(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
            timer = new Timer(ExecuteAsync, null, 0, 10000);
        }

        private async void ExecuteAsync(object state)
        {
            // Kod do wykonania co 10 sekund
            Console.WriteLine("XXX Pobieranie danych z API i zapisywanie ich do bazy danych...");
            await weatherService.GetWeatherSet();

            // Tutaj możesz umieścić kod, który ma być wykonywany co 10 sekund.
            // Pamiętaj, żeby kod w tej metodzie był zabezpieczony przed wyjątkami i miał odpowiednią obsługę błędów.
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
                if (!string.IsNullOrEmpty(Request.Form["submit"].ToString()))
                {
                    openWeatherMap.apiResponse = "► Select City";
                }
            }
            return View(openWeatherMap);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            await weatherService.GetWeatherSet();
            return View();
        }
    }
}