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
        public tourism_photo hotel_photos { get; set; }
    }
}