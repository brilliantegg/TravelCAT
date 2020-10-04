using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class BadgeDetails
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public int BadgeId { get; set; }

        public virtual Badge Badge { get; set; }
        public virtual Member Member { get; set; }
    }
}
