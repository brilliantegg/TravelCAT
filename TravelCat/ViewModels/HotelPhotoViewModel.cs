using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class HotelPhotoViewModel
    {
        public hotel hotel { get; set; }
        public List<tourism_photo> hotel_photos { get; set; }
    }
}