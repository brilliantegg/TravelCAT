using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class SpotPhotoViewModel
    {
        public spot spot { get; set; }
        public tourism_photo spot_photos { get; set; }
    }
}