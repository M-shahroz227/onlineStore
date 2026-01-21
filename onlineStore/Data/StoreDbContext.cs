using Microsoft.EntityFrameworkCore;
using onlineStore.Model;

namespace onlineStore.Data
{
    // Rename this class to avoid conflict with Microsoft.EntityFrameworkCore.DbContext
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {  
        }
       public DbSet<Register> registers { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Product> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
