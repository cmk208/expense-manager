using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EM.DB.Models
{
    public class SubCategory
    {
        [Key]
        [Column(Order = 0)]
        public Guid SubCategoryID { get; set; }
        
        [Column(Order = 1)]
        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }

        [Required]
        [StringLength(200)]
        public string SubCategoryName { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Category Category { get; set; }
    }
}