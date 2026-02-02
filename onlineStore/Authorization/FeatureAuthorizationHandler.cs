using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using System.Security.Claims;

namespace onlineStore.Authorization
{
    public class FeatureAuthorizationHandler
        : AuthorizationHandler<FeatureRequirement>
    {
        private readonly StoreDbContext _context;

        public FeatureAuthorizationHandler(StoreDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            FeatureRequirement requirement)
        {
            Console.WriteLine("🔥 FeatureAuthorizationHandler CALLED");

            // 1️⃣ Get UserId from JWT (standard claim)
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                Console.WriteLine("❌ UserId claim missing");
                return;
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                Console.WriteLine("❌ Invalid UserId claim");
                return;
            }

            // 2️⃣ Get user's features from DB safely
            var allowed = await _context.UserFeatures
                .Include(uf => uf.Feature)       // Important: load Feature
                .AnyAsync(uf =>
                    uf.UserId == userId &&
                    uf.Feature != null &&          // null check
                    uf.Feature.Code == requirement.Feature &&
                    uf.Feature.IsActive);

            // 3️⃣ Authorize if allowed
            if (allowed)
            {
                Console.WriteLine($"✅ User {userId} authorized for {requirement.Feature}");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine($"❌ User {userId} NOT authorized for {requirement.Feature}");
            }
        }
    }
}
