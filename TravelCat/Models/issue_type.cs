namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class issue_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public issue_type()
        {
            issues = new HashSet<issue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int issue_id { get; set; }

        [Required]
        [StringLength(10)]
        public string issue_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<issue> issues { get; set; }
    }
}
