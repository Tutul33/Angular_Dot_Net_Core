﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DataContext
{
    public partial class TblCity
    {        
        public int CityId { get; set; }
        public int? CountryId { get; set; }
        public string CityName { get; set; }
        public bool? IsActive { get; set; }       
    }
}
