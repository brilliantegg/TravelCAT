namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class follow_list
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        public string member_id { get; set; }

        [Required]
        [StringLength(7)]
        public string followed_id { get; set; }

        public DateTime follow_date { get; set; }

        public virtual member member { get; set; }
    }
}
