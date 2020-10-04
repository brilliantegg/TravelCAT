using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Issue
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public int AdminId { get; set; }
        public int IssueId { get; set; }
        public DateTime ReportDate { get; set; }
        public string IssueContent { get; set; }
        public string IssueResult { get; set; }
        public bool? IssueStatus { get; set; }
        public DateTime? ResolveDate { get; set; }
        public string ProblemId { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual IssueType IssueNavigation { get; set; }
        public virtual Member Member { get; set; }
    }
}
