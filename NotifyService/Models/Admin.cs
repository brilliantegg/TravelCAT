using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Issue = new HashSet<Issue>();
        }

        public int AdminId { get; set; }
        public string AdminAccount { get; set; }
        public string AdminPassword { get; set; }
        public string AdminEmail { get; set; }
        public bool? EmailConfirmed { get; set; }

        public virtual ICollection<Issue> Issue { get; set; }
    }
}
