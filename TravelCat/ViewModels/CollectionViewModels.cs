using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class CollectionViewModels
    {
      
        public member member { get; set; }
        public member_profile member_profile { get; set; }
        public List<collections_detail> collections_detail { get; set; }
        public List<activity> activity { get; set; }
        public List<hotel> hotel { get; set; }
        public List<restaurant> restaurant { get; set; }
        public List<spot> spot { get; set; }

        public List<a_c> a_c { get; set; }

    }

    public class a_c
    {
        public string id;
    }
}