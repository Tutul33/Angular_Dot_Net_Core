using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DataContext
{
    public partial class TblPersonalInfo
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
    }
}
