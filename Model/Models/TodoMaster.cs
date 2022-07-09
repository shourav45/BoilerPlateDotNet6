using System;
using System.Collections.Generic;

namespace Model.Models
{
    public partial class TodoMaster
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
