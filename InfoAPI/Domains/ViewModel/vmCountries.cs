using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.ViewModel
{
    public class vmCountries
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public List<vmCities> cities { get; set; }
    }
    public class vmCities
    {
        public int? CountryId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
