using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenWeather.BusinessLogic.Helpers;
using OpenWeather.BusinessLogic.Models;
using OpenWeather.BusinessLogic.Services;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly int _timeInterval;

    public WeatherBackgroundService(
        IServiceScopeFactory scopeFactory, 
        IConfiguration configuration
        )
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _timeInterval = int.Parse(_configuration.GetSection("TimeInterval").Value);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await FetchApiWeatherSet(scope.ServiceProvider, stoppingToken); 
            }
            await Task.Delay(_timeInterval, stoppingToken);
        }
    }

    private async Task FetchApiWeatherSet(IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        var weatherFetchService = serviceProvider.GetRequiredService<WeatherFetchService>(); 
        var cities = serviceProvider.GetRequiredService<Cities>();

        foreach (var city in cities.Read())
        {
            if (stoppingToken.IsCancellationRequested) break;

            var weatherInfo = await weatherFetchService.FetchAndCombineWeatherData(city.Name, city.AirlyId);

            await weatherFetchService.AddWeatherInfo(weatherInfo);
        }
    }
}
