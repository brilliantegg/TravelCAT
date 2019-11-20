using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class MemberIndexViewModels
    {
        public member member { get; set; }
        public member_profile member_profile { get; set; }

        public List<badge_details> badge_details { get; set; }

        public List<comment> comment { get; set; }

        public List<follow_list> follow_list { get; set; }


    }
}