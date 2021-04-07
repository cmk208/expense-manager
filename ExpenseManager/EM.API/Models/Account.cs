using System;

namespace EM.API.Models
{
    public class Account
    {
        public Guid AccountID { get; set; }
        public Guid AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}