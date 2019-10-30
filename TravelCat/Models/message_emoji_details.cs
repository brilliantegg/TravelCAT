namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class message_emoji_details
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
        public long msg_id { get; set; }

        public int emoji_id { get; set; }

        public virtual emoji emoji { get; set; }

        public virtual member member { get; set; }

        public virtual message message { get; set; }
    }
}
