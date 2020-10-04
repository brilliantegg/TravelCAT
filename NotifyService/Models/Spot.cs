using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class Spot
    {
        public string SpotId { get; set; }
        public string SpotTitle { get; set; }
        public string SpotTel { get; set; }
        public string SpotIntro { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AdditionNote { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressDetail { get; set; }
        public string OpenTime { get; set; }
        public string TicketInfo { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string TravellingInfo { get; set; }
        public bool PageStatus { get; set; }
    }
}
