namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("restaurant")]
    public partial class restaurant
    {
        [Key]
        [StringLength(7)]
        public string restaurant_id { get; set; }

        [Required]
        [StringLength(30)]
        public string restaurant_title { get; set; }

        [StringLength(20)]
        public string restaurant_tel { get; set; }

        [Column(TypeName = "ntext")]
        public string restaurant_intro { get; set; }

        [Required]
        [StringLength(25)]
        public string latitude { get; set; }

        [Required]
        [StringLength(25)]
        public string longitude { get; set; }

        [Required]
        [StringLength(10)]
        public string city { get; set; }

        [Required]
        [StringLength(5)]
        public string district { get; set; }

        [Required]
        [StringLength(60)]
        public string address_detail { get; set; }

        [StringLength(80)]
        public string open_time { get; set; }

        public bool page_status { get; set; }
    }
}
