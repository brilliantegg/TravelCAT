using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Member
    {
        public Member()
        {
            BadgeDetails = new HashSet<BadgeDetails>();
            CollectionsDetail = new HashSet<CollectionsDetail>();
            Comment = new HashSet<Comment>();
            CommentEmojiDetails = new HashSet<CommentEmojiDetails>();
            FollowList = new HashSet<FollowList>();
            Issue = new HashSet<Issue>();
            Message = new HashSet<Message>();
            MessageEmojiDetails = new HashSet<MessageEmojiDetails>();
        }

        public string MemberId { get; set; }
        public string MemberAccount { get; set; }
        public string MemberPassword { get; set; }
        public bool MemberStatus { get; set; }

        public virtual MemberProfile MemberProfile { get; set; }
        public virtual ICollection<BadgeDetails> BadgeDetails { get; set; }
        public virtual ICollection<CollectionsDetail> CollectionsDetail { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<CommentEmojiDetails> CommentEmojiDetails { get; set; }
        public virtual ICollection<FollowList> FollowList { get; set; }
        public virtual ICollection<Issue> Issue { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<MessageEmojiDetails> MessageEmojiDetails { get; set; }
    }
}
