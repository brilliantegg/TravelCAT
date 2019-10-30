namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class comment_emoji_details
    {
        [Key]
        [Column(Order = 0)]
        public long id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        public string member_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long comment_id { get; set; }

        public int emoji_id { get; set; }

        public virtual comment comment { get; set; }

        public virtual emoji emoji { get; set; }

        public virtual member member { get; set; }
    }
}
