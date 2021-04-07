using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.DB.Models
{
    public class TransactionType
    {
        [Key]
        public Guid TransactionTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string TransactionTypeName { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}