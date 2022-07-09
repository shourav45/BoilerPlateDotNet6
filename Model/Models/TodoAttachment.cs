using System;
using System.Collections.Generic;

namespace Model.Models
{
    public partial class TodoAttachment
    {
        public int Id { get; set; }
        public int TodoId { get; set; }
        public string FilePath { get; set; } = null!;
        public string FileTitle { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
