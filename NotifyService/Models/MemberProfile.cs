using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class MemberProfile
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Nickname { get; set; }
        public DateTime CreateTime { get; set; }
        public string Nation { get; set; }
        public string City { get; set; }
        public string AddressDetail { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string Phone { get; set; }
        public string ProfilePhoto { get; set; }
        public string WebsiteLink { get; set; }
        public string BriefIntro { get; set; }
        public string CoverPhoto { get; set; }
        public int? MemberScore { get; set; }

        public virtual Member Member { get; set; }
    }
}
