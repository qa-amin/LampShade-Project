using AccountManagement.Domain.AccountAgg;
using AccountManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore
{
    public class AccountManagementDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public AccountManagementDbContext(DbContextOptions<AccountManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountMapping());
        }
    }
}