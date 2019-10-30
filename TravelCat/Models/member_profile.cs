namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class member_profile
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string member_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string member_name { get; set; }

        public bool? gender { get; set; }

        public DateTime birthday { get; set; }

        [Required]
        [StringLength(20)]
        public string nickname { get; set; }

        public DateTime create_time { get; set; }

        [Required]
        [StringLength(30)]
        public string nation { get; set; }

        [StringLength(60)]
        public string address_detail { get; set; }

        [Required]
        [StringLength(30)]
        public string city { get; set; }

        [Required]
        [StringLength(60)]
        public string email { get; set; }

        [Required]
        [StringLength(10)]
        public string phone { get; set; }

        [StringLength(100)]
        public string profile_photo { get; set; }

        [StringLength(150)]
        public string website_link { get; set; }

        [StringLength(300)]
        public string brief_intro { get; set; }

        [StringLength(100)]
        public string cover_photo { get; set; }

        public int? member_score { get; set; }

        public virtual member member { get; set; }
    }
}
