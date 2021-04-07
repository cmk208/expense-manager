using System;
using System.Collections.Generic;

namespace EM.API.Models
{
    public class Report
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}