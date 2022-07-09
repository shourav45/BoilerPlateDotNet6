using System;
using System.Collections.Generic;

namespace Model.Models
{
    public partial class UserAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string Cif { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
