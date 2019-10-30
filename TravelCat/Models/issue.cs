namespace TravelCat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("issue")]
    public partial class issue
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        public string member_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int admin_id { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int issue_id { get; set; }

        public DateTime report_date { get; set; }

        [Required]
        [StringLength(1000)]
        public string issue_content { get; set; }

        [StringLength(1000)]
        public string issue_result { get; set; }

        [StringLength(10)]
        public string issue_status { get; set; }

        public DateTime? resolve_date { get; set; }

        public virtual admin admin { get; set; }

        public virtual issue_type issue_type { get; set; }

        public virtual member member { get; set; }
    }
}
