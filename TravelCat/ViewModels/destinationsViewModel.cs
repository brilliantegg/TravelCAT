using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class destinationsViewModel
    {
        public  restaurant restaurant { get; set; }
        public hotel hotel { get; set; }
        public activity activity { get; set; }
        public spot spot { get; set; }
        public comment comment { get; set; }
        public member_profile member_profile { get; set; }
        public message message { get; set; }
    }
}