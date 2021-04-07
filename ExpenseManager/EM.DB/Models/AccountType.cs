using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.DB.Models
{
    public class AccountType
    {
        [Key]
        public Guid AccountTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string AccountTypeName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}