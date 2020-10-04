using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class IssueType
    {
        public IssueType()
        {
            Issue = new HashSet<Issue>();
        }

        public int IssueId { get; set; }
        public string IssueName { get; set; }

        public virtual ICollection<Issue> Issue { get; set; }
    }
}
