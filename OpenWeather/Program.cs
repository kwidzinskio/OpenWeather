using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenWeather.BusinessLogic.Helpers;
using OpenWeather.BusinessLogic.Models;
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
            builder.Services.AddHostedService<WeatherBackgroundService>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<WeatherFetchService>();
            builder.Services.AddSingleton<Cities>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather and Pollution API", Version = "v1" });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather and Pollution API"));
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