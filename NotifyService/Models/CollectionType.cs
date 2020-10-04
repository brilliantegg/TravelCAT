using System;
using System.Collections.Generic;

namespace NotifyService.Models
{
    public partial class CollectionType
    {
        public CollectionType()
        {
            CollectionsDetail = new HashSet<CollectionsDetail>();
        }

        public int CollectionTypeId { get; set; }
        public string CollectionTypeTitle { get; set; }

        public virtual ICollection<CollectionsDetail> CollectionsDetail { get; set; }
    }
}
