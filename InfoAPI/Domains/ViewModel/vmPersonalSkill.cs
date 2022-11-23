using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class vmPersonalSkill
    {
        public int UserSkillId { get; set; }
        public int? UserId { get; set; }
        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsChecked { get; set; }
    }
}
