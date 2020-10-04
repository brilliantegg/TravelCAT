using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Hotel
    {
        public string HotelId { get; set; }
        public string HotelTitle { get; set; }
        public string HotelTel { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string HotelIntro { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressDetail { get; set; }
        public bool PageStatus { get; set; }
    }
}
