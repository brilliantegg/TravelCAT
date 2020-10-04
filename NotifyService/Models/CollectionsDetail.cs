using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class CollectionsDetail
    {
        public int CollectionId { get; set; }
        public string MemberId { get; set; }
        public string TourismId { get; set; }
        public bool Privacy { get; set; }
        public int CollectionTypeId { get; set; }

        public virtual CollectionType CollectionType { get; set; }
        public virtual Member Member { get; set; }
    }
}
