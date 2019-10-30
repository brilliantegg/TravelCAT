namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class collections_detail
    {
        [Key]
        [Column(Order = 0)]
        public int collection_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        public string member_id { get; set; }

        [Required]
        [StringLength(7)]
        public string tourism_id { get; set; }

        public bool privacy { get; set; }

        public int collection_type_id { get; set; }

        public virtual collection_type collection_type { get; set; }

        public virtual member member { get; set; }
    }
}
