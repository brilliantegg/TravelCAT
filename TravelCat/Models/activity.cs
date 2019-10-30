namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("activity")]
    public partial class activity
    {
        [Key]
        [StringLength(7)]
        public string activity_id { get; set; }

        [Required]
        [StringLength(30)]
        public string activity_title { get; set; }

        [StringLength(20)]
        public string activity_tel { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string activity_intro { get; set; }

        [Required]
        [StringLength(25)]
        public string longitude { get; set; }

        [Required]
        [StringLength(25)]
        public string latitude { get; set; }

        [Required]
        [StringLength(10)]
        public string city { get; set; }

        [Required]
        [StringLength(10)]
        public string district { get; set; }

        [Required]
        [StringLength(60)]
        public string address_detail { get; set; }

        [Required]
        [StringLength(30)]
        public string end_date { get; set; }

        [Required]
        [StringLength(30)]
        public string begin_date { get; set; }

        [StringLength(50)]
        public string organizer { get; set; }

        [StringLength(150)]
        public string website { get; set; }

        [StringLength(200)]
        public string transport_info { get; set; }

        public bool page_status { get; set; }
    }
}
