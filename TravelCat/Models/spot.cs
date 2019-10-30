namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("spot")]
    public partial class spot
    {
        [Key]
        [StringLength(7)]
        public string spot_id { get; set; }

        [Required]
        [StringLength(30)]
        public string spot_title { get; set; }

        [Required]
        [StringLength(20)]
        public string spot_tel { get; set; }

        [Column(TypeName = "ntext")]
        public string spot_intro { get; set; }

        [Required]
        [StringLength(25)]
        public string longitude { get; set; }

        [Required]
        [StringLength(25)]
        public string latitude { get; set; }

        [StringLength(150)]
        public string addition_note { get; set; }

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

        [StringLength(30)]
        public string ticket_info { get; set; }

        public DateTime? update_date { get; set; }

        public bool page_status { get; set; }
    }
}
