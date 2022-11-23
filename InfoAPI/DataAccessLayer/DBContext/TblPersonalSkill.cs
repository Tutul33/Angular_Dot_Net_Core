using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.DBContext
{
    public partial class TblPersonalSkill
    {
        public int UserSkillId { get; set; }
        public int? UserId { get; set; }
        public int? SkillId { get; set; }
        public bool? IsActive { get; set; }
    }
}
