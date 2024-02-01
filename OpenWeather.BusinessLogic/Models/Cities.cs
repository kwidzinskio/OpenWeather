using System;
using System.Collections.Generic;

namespace OpenWeather.BusinessLogic.Models
{
    public class City
    {
        public string Name { get; }
        public int AirlyId { get; }

        public City(string name, int airlyId)
        {
            Name = name;
            AirlyId = airlyId;
        }
    }

    public class Cities
    {
        private readonly List<City> cities;

        public Cities()
        {
            cities = new List<City>
            {
                new City ("London", 114460 ),
                new City ("Paris", 16634 ),
                new City("Beijing", 81753),
                new City("Washington", 108674)
            };
        }

        public List<City> Read()
        {
            return cities;
        }

        public int? FindAirlyIdByName(string cityName)
        {
            var city = cities.Find(c => c.Name.Equals(cityName, StringComparison.OrdinalIgnoreCase));
            return city?.AirlyId;
        }
    }
}