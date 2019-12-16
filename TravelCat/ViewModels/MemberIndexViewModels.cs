using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class MemberIndexViewModels
    {
        public FollowerViewModels FollowerViewModels { get; set; }
        public destinationsViewModel destinationsViewModel { get; set; }
        public member member { get; set; }
        public member_profile member_profile { get; set; }
        public List<badge_details> badge_details { get; set; }
        public List<comment> comment { get; set; }
        public List<message> message { get; set; }
        public List<comment_emoji_details> comment_emoji_details { get; set; }
        public List<message_emoji_details> message_emoji_details { get; set; }
        public List<follow_list> follow { get; set; }
        public List<follow_list> followed { get; set; }
        public List<follow_list> follow_list { get; set; }
        public List<collections_detail> collections_detail { get; set; }
        public List<activity> activity { get; set; }
        public List<hotel> hotel { get; set; }
        public List<restaurant> restaurant { get; set; }
        public List<spot> spot { get; set; }
    }

}