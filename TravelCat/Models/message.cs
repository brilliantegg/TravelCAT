namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("message")]
    public partial class message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public message()
        {
            message_emoji_details = new HashSet<message_emoji_details>();
        }

        [Key]
        public long msg_id { get; set; }

        public DateTime msg_time { get; set; }

        [Required]
        [StringLength(1000)]
        public string msg_content { get; set; }

        public long comment_id { get; set; }

        [Required]
        [StringLength(7)]
        public string member_id { get; set; }

        public virtual comment comment { get; set; }

        public virtual member member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<message_emoji_details> message_emoji_details { get; set; }
    }
}
