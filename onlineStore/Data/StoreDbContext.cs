using Microsoft.EntityFrameworkCore;
using onlineStore.Model;

namespace onlineStore.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        // =========================
        // MAIN TABLES
        // =========================
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Product> Products { get; set; }

        // =========================
        // JUNCTION TABLES
        // =========================
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }
        public DbSet<UserFeature> UserFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================================================
            // USER ↔ ROLE (Many-to-Many)
            // ==================================================
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // ==================================================
            // ROLE ↔ FEATURE (Many-to-Many)
            // ==================================================
            modelBuilder.Entity<RoleFeature>()
                .HasKey(rf => new { rf.RoleId, rf.FeatureId });

            modelBuilder.Entity<RoleFeature>()
                .HasOne(rf => rf.Role)
                .WithMany(r => r.RoleFeatures)
                .HasForeignKey(rf => rf.RoleId);

            modelBuilder.Entity<RoleFeature>()
                .HasOne(rf => rf.Feature)
                .WithMany(f => f.RoleFeatures)
                .HasForeignKey(rf => rf.FeatureId);

            // ==================================================
            // USER ↔ FEATURE (DIRECT OVERRIDE)
            // ==================================================
            modelBuilder.Entity<UserFeature>()
                .HasKey(uf => new { uf.UserId, uf.FeatureId });

            modelBuilder.Entity<UserFeature>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFeatures)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFeature>()
                .HasOne(uf => uf.Feature)
                .WithMany(f => f.UserFeatures)
                .HasForeignKey(uf => uf.FeatureId);
        }
    }
}
