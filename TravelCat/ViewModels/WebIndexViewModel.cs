using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class WebIndexViewModel
    {
        public activity activity { get; set; }
        public hotel hotel { get; set; }
        public restaurant restaurant { get; set; }
        public spot spot { get; set; }
        public member member { get; set; }
        public comment comment { get; set; }
    }
}