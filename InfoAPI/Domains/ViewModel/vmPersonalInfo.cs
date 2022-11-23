using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class vmPersonalInfo
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        [Required]
        [StringLength(250)]
        public string FullName { get; set; }
        [Required]
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        [Required]
        public int? CityId { get; set; }
        public string CityName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        [Required]
        public List<vmPersonalSkill> skilList { get; set; }
        public List<vmAttachment> attachments { get; set; }
        public string WebRootPath { get; set; }
    }
}
