using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelCat.Models;

namespace TravelCat.ViewModels
{
    public class ActivityPhotoViewModel
    {
        public activity activity { get; set; }
        public tourism_photo activity_photos { get; set; }
    }
}