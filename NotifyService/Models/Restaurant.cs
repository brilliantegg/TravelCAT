using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantTitle { get; set; }
        public string RestaurantTel { get; set; }
        public string RestaurantIntro { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressDetail { get; set; }
        public string OpenTime { get; set; }
        public bool PageStatus { get; set; }
    }
}
