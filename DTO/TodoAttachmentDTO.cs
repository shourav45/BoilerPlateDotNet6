using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TodoAttachmentDTO
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
