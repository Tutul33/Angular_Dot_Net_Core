using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ViewModel
{
    public class vmAttachment
    {
        public int AttachmentId { get; set; }
        public int? UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool? IsActive { get; set; }
        public string FileData { get; set; }
        public string Extension { get; set; }
        public string attachmentFile { get; set; }
    }
}
