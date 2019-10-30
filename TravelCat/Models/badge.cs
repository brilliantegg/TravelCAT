namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("badge")]
    public partial class badge
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public badge()
        {
            badge_details = new HashSet<badge_details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int badge_id { get; set; }

        [Required]
        [StringLength(15)]
        public string badge_title { get; set; }

        [Required]
        [StringLength(100)]
        public string badge_photo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<badge_details> badge_details { get; set; }
    }
}
