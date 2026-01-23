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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }
        public DbSet<UserFeature> UserFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<RoleFeature>()
                .HasKey(x => new { x.RoleId, x.FeatureId });

            modelBuilder.Entity<UserFeature>()
                .HasKey(x => new { x.UserId, x.FeatureId });
        }
    }
}
