using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class RestaurantPhotoViewModel
    {
        public restaurant restaurant { get; set; }
        public tourism_photo restaurant_photos { get; set; }
    }
}