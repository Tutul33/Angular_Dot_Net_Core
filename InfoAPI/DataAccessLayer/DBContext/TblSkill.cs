﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.DBContext
{
    public partial class TblSkill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public bool? IsActive { get; set; }
    }
}
