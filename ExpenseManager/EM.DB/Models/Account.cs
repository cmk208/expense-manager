using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EM.DB.Models
{
    public class Account
    {
        [Key]
        [Column(Order = 0)]
        public Guid AccountID { get; set; }

        [Required]
        [Column(Order = 1)]
        [ForeignKey("AccountType")]
        public Guid AccountTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string AccountName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }


        public virtual AccountType AccountType { get; set; }

        [InverseProperty("FromAccount")]
        public virtual ICollection<Transaction> FromTransactions { get; set; }

        [InverseProperty("ToAccount")]
        public virtual ICollection<Transaction> ToTransactions { get; set; }
    }
}