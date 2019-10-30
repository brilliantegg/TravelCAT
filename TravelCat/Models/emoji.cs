namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("emoji")]
    public partial class emoji
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public emoji()
        {
            comment_emoji_details = new HashSet<comment_emoji_details>();
            message_emoji_details = new HashSet<message_emoji_details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int emoji_id { get; set; }

        [Required]
        [StringLength(10)]
        public string emoji_title { get; set; }

        [Required]
        [StringLength(100)]
        public string emoji_pic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment_emoji_details> comment_emoji_details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<message_emoji_details> message_emoji_details { get; set; }
    }
}
