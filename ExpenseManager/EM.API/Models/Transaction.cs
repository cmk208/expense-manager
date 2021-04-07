using System;

namespace EM.API.Models
{
    public class Transaction
    {
        public Guid TransactionID { get; set; }
        public Guid? FromTransactionID { get; set; }
        public Guid TransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
        public Guid FromAccountID { get; set; }
        public string FromAccountName { get; set; }
        public Guid? ToAccountID { get; set; }
        public string ToAccountName { get; set; }
        public Guid? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Guid? SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public DateTime TransactionDate { get; set; }
        public TimeSpan TransactionTime { get; set; }
        public bool Operator { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}