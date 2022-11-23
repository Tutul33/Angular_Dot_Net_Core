using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DataContext
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            TblCities = new HashSet<TblCity>();
            TblPersonalInfos = new HashSet<TblPersonalInfo>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<TblCity> TblCities { get; set; }
        public virtual ICollection<TblPersonalInfo> TblPersonalInfos { get; set; }
    }
}
