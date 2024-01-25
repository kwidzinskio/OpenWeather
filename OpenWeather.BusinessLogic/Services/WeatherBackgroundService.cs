using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenWeather.BusinessLogic.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly int timeInterval = 10000; 

    public WeatherBackgroundService(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                await weatherService.GetWeatherSet();
            }
            Console.WriteLine("...");
            await Task.Delay(timeInterval, stoppingToken);
        }
    }
}
