namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hotel")]
    public partial class hotel
    {
        [Key]
        [StringLength(7)]
        public string hotel_id { get; set; }

        [Required]
        [StringLength(30)]
        public string hotel_title { get; set; }

        [Required]
        [StringLength(20)]
        public string hotel_tel { get; set; }

        [Required]
        [StringLength(25)]
        public string longitude { get; set; }

        [Required]
        [StringLength(25)]
        public string latitude { get; set; }

        [Column(TypeName = "ntext")]
        public string hotel_intro { get; set; }

        [StringLength(150)]
        public string website { get; set; }

        [Required]
        [StringLength(10)]
        public string city { get; set; }

        [Required]
        [StringLength(5)]
        public string district { get; set; }

        [Required]
        [StringLength(60)]
        public string address_detail { get; set; }

        public bool page_status { get; set; }
    }
}
