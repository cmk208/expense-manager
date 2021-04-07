using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EM.DB.Models
{
    public class Category
    {
        [Key]
        [Column(Order = 0)]
        public Guid CategoryID { get; set; }

        [Column(Order = 1)]
        [ForeignKey("TransactionType")]
        public Guid TransactionTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual TransactionType TransactionType { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}