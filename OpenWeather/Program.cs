using Microsoft.EntityFrameworkCore;
using OpenWeather.BusinessLogic.Helpers;
using OpenWeather.BusinessLogic.Services;
using OpenWeather.DatabaseLayer.Context;
using OpenWeather.DatabaseLayer.Repositories;

namespace OpenWeather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<WeatherContext>(c =>
                c.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseContextConnectionString")));
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddScoped<IWeatherInfoRepository, WeatherInfoRepository>();
            builder.Services.AddTransient<IWeatherInfoRepositoryFactory, WeatherInfoRepositoryFactory>();
            //builder.Services.AddHostedService<WeatherInfoFetch>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}