using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class WebIndexViewModel
    {
        public List<activity> activity { get; set; }
        public List<hotel> hotel { get; set; }
        public List<restaurant> restaurant { get; set; }
        public List<spot> spot { get; set; }
        public member member { get; set; }
        public List<comment> comment { get; set; }
        public List<collections_detail> collections_Details { get; set; }
        public List<follow_list> follow_list { get; set; }

    }
}