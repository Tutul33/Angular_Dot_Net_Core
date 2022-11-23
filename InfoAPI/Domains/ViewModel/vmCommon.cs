using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class vmCommon
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public bool IsPaging { get; set; }
        public string values { get; set; }
        public int id { get; set; }
    }
}
