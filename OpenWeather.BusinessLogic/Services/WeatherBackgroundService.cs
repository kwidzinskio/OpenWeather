using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenWeather.BusinessLogic.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly int _timeInterval = 600000; 

    public WeatherBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                await weatherService.FetchApiWeatherSet();
            }
            await Task.Delay(_timeInterval, stoppingToken);
        }
    }
}
