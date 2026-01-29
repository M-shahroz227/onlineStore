using onlineStore.Data;
using onlineStore.Model;
using System.Linq;

public static class DataSeeder
{
    public static void Seed(StoreDbContext context)
    {
        // =========================
        // 1️⃣ FEATURES
        // =========================
        if (!context.Features.Any())
        {
            context.Features.AddRange(
                new Feature { Code = "PRODUCT_VIEW", Name = "View Products", IsActive = true },
                new Feature { Code = "PRODUCT_CREATE", Name = "Create Product", IsActive = true },
                new Feature { Code = "PRODUCT_UPDATE", Name = "Update Product", IsActive = true },
                new Feature { Code = "PRODUCT_DELETE", Name = "Delete Product", IsActive = true }
            );
            context.SaveChanges();
        }

        // =========================
        // 2️⃣ ROLES
        // =========================
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "User" },
                new Role { Name = "Manager" },
                new Role { Name = "Admin" }
            );
            context.SaveChanges();
        }

        // =========================
        // 3️⃣ USERS (WITH PASSWORD)
        // =========================
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    UserName = "user",
                    PasswordHash = PasswordHelper.Hash("User@123")
                },
                new User
                {
                    UserName = "manager",
                    PasswordHash = PasswordHelper.Hash("Manager@123")
                },
                new User
                {
                    UserName = "admin",
                    PasswordHash = PasswordHelper.Hash("Admin@123")
                }
            );
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error seeding users: " + ex.Message);
            }

            // =========================
            // 4️⃣ FETCH ENTITIES
            // =========================
            var userRole = context.Roles.First(r => r.Name == "User");
            var managerRole = context.Roles.First(r => r.Name == "Manager");
            var adminRole = context.Roles.First(r => r.Name == "Admin");

            var user = context.Users.First(u => u.UserName == "user");
            var manager = context.Users.First(u => u.UserName == "manager");
            var admin = context.Users.First(u => u.UserName == "admin");

            var view = context.Features.First(f => f.Code == "PRODUCT_VIEW");
            var create = context.Features.First(f => f.Code == "PRODUCT_CREATE");
            var update = context.Features.First(f => f.Code == "PRODUCT_UPDATE");
            var delete = context.Features.First(f => f.Code == "PRODUCT_DELETE");

            // =========================
            // 5️⃣ USER ↔ ROLE
            // =========================
            if (!context.UserRoles.Any())
            {
                context.UserRoles.AddRange(
                    new UserRole { UserId = user.Id, RoleId = userRole.Id },
                    new UserRole { UserId = manager.Id, RoleId = managerRole.Id },
                    new UserRole { UserId = admin.Id, RoleId = adminRole.Id }
                );
                context.SaveChanges();
            }

            // =========================
            // 6️⃣ ROLE ↔ FEATURE
            // =========================
            if (!context.RoleFeatures.Any())
            {
                context.RoleFeatures.AddRange(
                    // User
                    new RoleFeature { RoleId = userRole.Id, FeatureId = view.Id },

                    // Manager
                    new RoleFeature { RoleId = managerRole.Id, FeatureId = view.Id },
                    new RoleFeature { RoleId = managerRole.Id, FeatureId = create.Id },
                    new RoleFeature { RoleId = managerRole.Id, FeatureId = update.Id },

                    // Admin
                    new RoleFeature { RoleId = adminRole.Id, FeatureId = view.Id },
                    new RoleFeature { RoleId = adminRole.Id, FeatureId = create.Id },
                    new RoleFeature { RoleId = adminRole.Id, FeatureId = update.Id },
                    new RoleFeature { RoleId = adminRole.Id, FeatureId = delete.Id }
                );
                context.SaveChanges();
            }

            // =========================
            // 7️⃣ USER ↔ FEATURE (OVERRIDE)
            // =========================
            if (!context.UserFeatures.Any())
            {
                context.UserFeatures.Add(
                    new UserFeature
                    {
                        UserId = manager.Id,
                        FeatureId = delete.Id // Manager extra permission
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
