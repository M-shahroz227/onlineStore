using onlineStore.Data;
using onlineStore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public static class DataSeeder
{
    public static void Seed(StoreDbContext context)
    {
        context.Database.EnsureCreated(); // Make sure DB exists

        // =========================
        // 1️⃣ FEATURES
        // =========================
        if (!context.Features.Any())
        {
            context.Features.AddRange(
                new Feature { Code = AppFeatures.PRODUCT_VIEW, Name = "View Products", IsActive = true },
                new Feature { Code = AppFeatures.PRODUCT_CREATE, Name = "Create Product", IsActive = true },
                new Feature { Code = AppFeatures.PRODUCT_UPDATE, Name = "Update Product", IsActive = true },
                new Feature { Code = AppFeatures.PRODUCT_DELETE, Name = "Delete Product", IsActive = true }
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
        // 3️⃣ USERS
        // =========================
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { UserName = "user", PasswordHash = PasswordHelper.Hash("User@123") },
                new User { UserName = "manager", PasswordHash = PasswordHelper.Hash("Manager@123") },
                new User { UserName = "admin", PasswordHash = PasswordHelper.Hash("Admin@123") }
            );
            context.SaveChanges();
        }

        // =========================
        // 4️⃣ FETCH DATA
        // =========================
        var user = context.Users.First(x => x.UserName == "user");
        var manager = context.Users.First(x => x.UserName == "manager");
        var admin = context.Users.First(x => x.UserName == "admin");

        var userRole = context.Roles.First(x => x.Name == "User");
        var managerRole = context.Roles.First(x => x.Name == "Manager");
        var adminRole = context.Roles.First(x => x.Name == "Admin");

        var view = context.Features.First(x => x.Code == "PRODUCT_VIEW");
        var create = context.Features.First(x => x.Code == "PRODUCT_CREATE");
        var update = context.Features.First(x => x.Code == "PRODUCT_UPDATE");
        var delete = context.Features.First(x => x.Code == "PRODUCT_DELETE");

        // =========================
        // 5️⃣ USER ↔ ROLE
        // =========================
        AddUserRole(context, user.Id, userRole.Id);
        AddUserRole(context, manager.Id, managerRole.Id);
        AddUserRole(context, admin.Id, adminRole.Id);
        context.SaveChanges();

        // =========================
        // 6️⃣ ROLE ↔ FEATURE
        // =========================
        // USER → View only
        AddRoleFeature(context, userRole.Id, view.Id);

        // MANAGER → Full CRUD
        AddRoleFeature(context, managerRole.Id, view.Id);
        AddRoleFeature(context, managerRole.Id, create.Id);
        AddRoleFeature(context, managerRole.Id, update.Id);
        AddRoleFeature(context, managerRole.Id, delete.Id);

        // ADMIN → View, Create, Update (no delete)
        AddRoleFeature(context, adminRole.Id, view.Id);
        AddRoleFeature(context, adminRole.Id, create.Id);
        AddRoleFeature(context, adminRole.Id, update.Id);

        context.SaveChanges();

        // =========================
        // 7️⃣ USER ↔ FEATURE (Direct override)
        // =========================
        AddUserFeature(context, user.Id, view.Id);               // user → only view
        AddUserFeature(context, manager.Id, view.Id);            // manager → all
        AddUserFeature(context, manager.Id, create.Id);
        AddUserFeature(context, manager.Id, update.Id);
        AddUserFeature(context, manager.Id, delete.Id);
        AddUserFeature(context, admin.Id, view.Id);             // admin → no delete
        AddUserFeature(context, admin.Id, create.Id);
        AddUserFeature(context, admin.Id, update.Id);

        context.SaveChanges();

        // =========================
        // 8️⃣ PRODUCTS
        // =========================
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Title = "Samsung Galaxy S23", Price = 219999, Stock = 15, Description = "Flagship Samsung phone with AMOLED display" },
                new Product { Title = "iPhone 14 Pro", Price = 349999, Stock = 10, Description = "Apple iPhone with A16 Bionic chip" },
                new Product { Title = "Xiaomi Redmi Note 13", Price = 69999, Stock = 25, Description = "Best budget Android smartphone" },
                new Product { Title = "Infinix Zero 30", Price = 89999, Stock = 20, Description = "Mid-range phone with strong camera" }
            );
            context.SaveChanges();
        }
    }

    // =========================
    // HELPERS
    // =========================
    private static void AddUserRole(StoreDbContext context, int userId, int roleId)
    {
        if (!context.UserRoles.Any(x => x.UserId == userId && x.RoleId == roleId))
        {
            context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
        }
    }

    private static void AddRoleFeature(StoreDbContext context, int roleId, int featureId)
    {
        if (!context.RoleFeatures.Any(x => x.RoleId == roleId && x.FeatureId == featureId))
        {
            context.RoleFeatures.Add(new RoleFeature { RoleId = roleId, FeatureId = featureId });
        }
    }

    private static void AddUserFeature(StoreDbContext context, int userId, int featureId)
    {
        if (!context.UserFeatures.Any(x => x.UserId == userId && x.FeatureId == featureId))
        {
            context.UserFeatures.Add(new UserFeature { UserId = userId, FeatureId = featureId });
        }
    }
}
