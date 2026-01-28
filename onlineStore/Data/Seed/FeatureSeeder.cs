using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using onlineStore.Authorization;
using onlineStore.Common;
using onlineStore.Model;
using AppFeatures = onlineStore.Common.AppFeatures;

namespace onlineStore.Data.Seed
{
    public static class FeatureSeeder
    {
        // Seed features into the database
        public static void Seed(StoreDbContext context)
        {
            if (context.Features.Any())
                return;

            var features = new[]
            {
                new Feature
                {
                    Code = AppFeatures.PRODUCT_VIEW,
                    Name = "View Products",
                    Description = "Allows viewing of products",
                    IsActive = true
                },
                new Feature
                {
                    Code = AppFeatures.PRODUCT_CREATE,
                    Name = "Create Product",
                    Description = "Allows creation of new products",
                    IsActive = true
                },
                new Feature
                {
                    Code = AppFeatures.PRODUCT_DELETE,
                    Name = "Delete Product",
                    Description = "Allows deletion of products",
                    IsActive = false
                }
            };

            context.Features.AddRange(features);
            context.SaveChanges();
        }

        // Optional: register feature-based authorization policies dynamically
        public static void RegisterFeaturePolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var featureNames = new[]
                {
                    AppFeatures.PRODUCT_VIEW,
                    AppFeatures.PRODUCT_CREATE,
                    AppFeatures.PRODUCT_DELETE
                };

                foreach (var feature in featureNames)
                {
                    options.AddPolicy($"Feature.{feature}", policy =>
                        policy.Requirements.Add(new FeatureRequirement(feature)));
                }
            });
        }
    }
}
