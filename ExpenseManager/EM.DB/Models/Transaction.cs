using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EM.DB.Models
{
    public class Transaction
    {
        [Key]
        [Column(Order = 0)]
        public Guid TransactionID { get; set; }

        [Column(Order = 1)]
        [ForeignKey("FromTransaction")]
        public Guid? FromTransactionID { get; set; }

        [Required]
        [Column(Order = 2)]
        [ForeignKey("TransactionType")]
        public Guid TransactionTypeID { get; set; }
        
        [Required]
        [Column(Order = 3)]
        [ForeignKey("FromAccount")]
        public Guid FromAccountID { get; set; }

        [Column(Order = 4)]
        [ForeignKey("ToAccount")]
        public Guid? ToAccountID { get; set; }

        [Column(Order = 5)]
        [ForeignKey("Category")]
        public Guid? CategoryID { get; set; }

        [Column(Order = 6)]
        [ForeignKey("SubCategory")]
        public Guid? SubCategoryID { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        public TimeSpan TransactionTime { get; set; }

        [Required]
        public bool Operator { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(300)]
        public string Note { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Transaction FromTransaction { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual Account FromAccount { get; set; }
        public virtual Account ToAccount { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}