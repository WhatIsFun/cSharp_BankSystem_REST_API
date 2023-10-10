using cSharp_BankSystem_REST_API.Model;
using Microsoft.EntityFrameworkCore;

namespace cSharp_BankSystem_REST_API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
             : base(option)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{

        //    options.UseSqlServer("Data Source=(local);Initial Catalog=BankSys; Integrated Security=true; TrustServerCertificate=True");
        //}
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
