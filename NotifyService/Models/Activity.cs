using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Activity
    {
        public string ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityTel { get; set; }
        public string ActivityIntro { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressDetail { get; set; }
        public string EndDate { get; set; }
        public string BeginDate { get; set; }
        public string Organizer { get; set; }
        public string Website { get; set; }
        public string TransportInfo { get; set; }
        public bool PageStatus { get; set; }
    }
}
