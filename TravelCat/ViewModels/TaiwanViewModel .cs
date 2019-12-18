using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;
using X.PagedList.Mvc;
using X.PagedList;

namespace TravelCat.ViewModels
{
    public class TaiwanViewModel
    {
        public List<collections_detail> collections_detail { get; set; }
        public List<activity> activity { get; set; }
        public List<hotel> hotel { get; set; }
        public List<restaurant> restaurant { get; set; }
        public List<spot> spot { get; set; }
        public List<member_profile> member { get; set; }
        public List<comment> comment { get; set; }
        public List<spot> north { get; set; }
        public List<spot> middle { get; set; }
        public List<spot> south { get; set; }
        public List<spot> East { get; set; }
        public List<spot> island { get; set; }
        public List<activity> activity_north { get; set; }
        public List<activity> activity_middle { get; set; }
        public List<activity> activity_south { get; set; }
        public List<activity> activity_East { get; set; }
        public List<activity> activity_island { get; set; }
        public List<hotel> hotel_north { get; set; }
        public List<hotel> hotel_middle { get; set; }
        public List<hotel> hotel_south { get; set; }
        public List<hotel> hotel_East { get; set; }
        public List<hotel> hotel_island { get; set; }
        public List<restaurant> restaurant_north { get; set; }
        public List<restaurant> restaurant_middle { get; set; }
        public List<restaurant> restaurant_south { get; set; }
        public List<restaurant> restaurant_East { get; set; }
        public List<restaurant> restaurant_island { get; set; }
    }


}