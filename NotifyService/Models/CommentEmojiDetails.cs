using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class CommentEmojiDetails
    {
        public long Id { get; set; }
        public string MemberId { get; set; }
        public long CommentId { get; set; }
        public int EmojiId { get; set; }
        public string TourismId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Emoji Emoji { get; set; }
        public virtual Member Member { get; set; }
    }
}
