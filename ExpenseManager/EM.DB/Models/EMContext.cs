using System.Data.Entity;

namespace EM.DB.Models
{
    public class EMContext : DbContext
    {
        public EMContext() : base("name=EMDB") {}

        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }
}