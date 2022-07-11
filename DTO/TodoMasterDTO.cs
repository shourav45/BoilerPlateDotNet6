using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TodoMasterDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? AssignedTo { get; set; }
        public DateTime? AssignedOn { get; set; }
        public string? FeedBack { get; set; }
    }
}
