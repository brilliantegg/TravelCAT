using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Badge
    {
        public Badge()
        {
            BadgeDetails = new HashSet<BadgeDetails>();
        }

        public int BadgeId { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgePhoto { get; set; }

        public virtual ICollection<BadgeDetails> BadgeDetails { get; set; }
    }
}
