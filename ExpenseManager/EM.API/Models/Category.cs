using System;

namespace EM.API.Models
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public Guid TransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
        public string CategoryName { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}