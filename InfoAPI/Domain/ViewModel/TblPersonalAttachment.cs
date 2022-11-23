using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.DataContext
{
    public partial class TblPersonalAttachment
    {
        public int AttachmentId { get; set; }
        public int? UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool? IsActive { get; set; }
    }
}
