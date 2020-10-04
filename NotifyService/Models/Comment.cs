using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentEmojiDetails = new HashSet<CommentEmojiDetails>();
            Message = new HashSet<Message>();
        }

        public long CommentId { get; set; }
        public string TourismId { get; set; }
        public string CommentTitle { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentPhoto { get; set; }
        public int CommentStayTotal { get; set; }
        public string TravelPartner { get; set; }
        public short CommentRating { get; set; }
        public string TravelMonth { get; set; }
        public bool CommentStatus { get; set; }
        public string MemberId { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<CommentEmojiDetails> CommentEmojiDetails { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
