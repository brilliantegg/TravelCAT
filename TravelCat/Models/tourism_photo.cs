namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tourism_photo
    {
        public long id { get; set; }

        [Required]
        [StringLength(7)]
        public string tourism_id { get; set; }

        [Column("tourism_photo")]
        [Required]
        [StringLength(60)]
        public string tourism_photo1 { get; set; }
    }
}
