namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("comment")]
    public partial class comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public comment()
        {
            comment_emoji_details = new HashSet<comment_emoji_details>();
            messages = new HashSet<message>();
        }

        [Key]
        public long comment_id { get; set; }

        [Required]
        [StringLength(7)]
        public string tourism_id { get; set; }

        [Required]
        [StringLength(20)]
        public string comment_title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string comment_content { get; set; }

        public DateTime comment_date { get; set; }

        [Required]
        [StringLength(100)]
        public string comment_photo { get; set; }

        public int comment_stay_total { get; set; }

        [Required]
        [StringLength(10)]
        public string travel_partner { get; set; }

        public short comment_rating { get; set; }

        [Required]
        [StringLength(10)]
        public string travel_month { get; set; }

        public bool comment_status { get; set; }

        [Required]
        [StringLength(7)]
        public string member_id { get; set; }

        public virtual member member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment_emoji_details> comment_emoji_details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<message> messages { get; set; }
    }
}
