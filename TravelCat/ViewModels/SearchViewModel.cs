using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;
using X.PagedList.Mvc;
using X.PagedList;

namespace TravelCat.ViewModels
{
    public class SearchViewModel
    {
        public List<activity> activity { get; set; }
        public List<hotel> hotel { get; set; }
        public List<restaurant> restaurant { get; set; }
        public List<spot> spot { get; set; }
        public List<member_profile> member { get; set; }
        public List<comment> comment { get; set; }
        public IPagedList<result_rating> show_ratings { get; set; }
        public List<result_rating> result_ratings { get; set; }
    }
    public class result_rating 
    {
        public string id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string rating { get; set; }
    }

}