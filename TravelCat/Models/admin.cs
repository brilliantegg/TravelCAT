namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("admin")]
    public partial class admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public admin()
        {
            issues = new HashSet<issue>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int admin_id { get; set; }

        [Required]
        [StringLength(15)]
        public string admin_account { get; set; }

        [Required]
        [StringLength(40)]
        public string admin_password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<issue> issues { get; set; }
    }
}
