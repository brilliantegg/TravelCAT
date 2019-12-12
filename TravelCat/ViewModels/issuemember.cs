using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class issuemember
    {
        public List<issue> issues { get; set; }
        public issue_type issue_type { get; set; }
        public member member { get; set; }
    }
}