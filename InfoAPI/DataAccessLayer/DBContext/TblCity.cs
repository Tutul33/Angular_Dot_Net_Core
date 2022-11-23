using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.DBContext
{
    public partial class TblCity
    {
        public TblCity()
        {
            TblPersonalInfos = new HashSet<TblPersonalInfo>();
        }

        public int CityId { get; set; }
        public int? CountryId { get; set; }
        public string CityName { get; set; }
        public bool? IsActive { get; set; }

        public virtual TblCountry Country { get; set; }
        public virtual ICollection<TblPersonalInfo> TblPersonalInfos { get; set; }
    }
}
