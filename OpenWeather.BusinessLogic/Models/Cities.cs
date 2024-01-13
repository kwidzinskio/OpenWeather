using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Models
{
    public class Cities
    {
        private readonly IEnumerable<string> cities;

        public Cities()
        {
            // Inicjalizacja listy 4 stringami
            cities = new List<string> { "London", "Paris", "Tokyo", "Washington" }.AsReadOnly();
        }

        public IEnumerable<string> ReadonlyCities
        {
            get { return cities; }
        }
    }
}
