using Microsoft.EntityFrameworkCore;
using NWBA_API.Models;

namespace NWBA_API.Data
{
    public class NwbaContext : DbContext
    {
        public NwbaContext(DbContextOptions<NwbaContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
            builder.Entity<Transaction>().
                HasOne(x => x.DestinationAccount).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
        }
    }
}
