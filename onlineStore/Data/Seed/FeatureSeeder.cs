using onlineStore.Common;
using onlineStore.Model;

namespace onlineStore.Data.Seed
{
    public static class FeatureSeeder
    {

        public static void Seed(StoreDbContext context)
        {
            if (context.Features.Any())
                return;

            context.Features.AddRange(
                new Feature
                {
                    Code = Common.AppFeatures.PRODUCT_VIEW,
                    Name = "View Products",
                    Description = "Allows viewing of products",
                    IsActive = true
                },
                new Feature
                {
                    Code = Common.AppFeatures.PRODUCT_CREATE,
                    Name = "Create Product",
                    Description = "Allows creation of new products",
                    IsActive = true
                },
                new Feature
                {
                    Code = Common.AppFeatures.PRODUCT_DELETE,
                    Name = "Delete Product",
                    Description = "Allows deletion of products",
                    IsActive = false
                }
            );

            context.SaveChanges();
        }
    }
}
