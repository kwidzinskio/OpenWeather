using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.DatabaseLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace OpenWeather.BusinessLogic.Helpers
{
    public class WeatherInfoFetch : BackgroundService
    {
    
        private readonly IConfiguration configuration;
        private readonly string apiKey;
        private readonly IServiceProvider services;
        private readonly IServiceScopeFactory scopeFactory;
        public WeatherInfoFetch(IServiceScopeFactory scopeFactory, IConfiguration _configuration, IServiceProvider _services)
        {

            configuration = _configuration;
            apiKey = configuration.GetSection("ApiKey").Value;
            services = _services;
            this.scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = services.CreateScope())
                {
                    Console.WriteLine("Pobieranie danych z API i zapisywanie ich do bazy danych...");
                    
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Poczekaj 10 sekund przed kolejnym uruchomieniem
            }
        }
    }
}
