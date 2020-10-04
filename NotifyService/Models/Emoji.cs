using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Emoji
    {
        public Emoji()
        {
            CommentEmojiDetails = new HashSet<CommentEmojiDetails>();
            MessageEmojiDetails = new HashSet<MessageEmojiDetails>();
        }

        public int EmojiId { get; set; }
        public string EmojiTitle { get; set; }
        public string EmojiPic { get; set; }

        public virtual ICollection<CommentEmojiDetails> CommentEmojiDetails { get; set; }
        public virtual ICollection<MessageEmojiDetails> MessageEmojiDetails { get; set; }
    }
}
