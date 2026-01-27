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
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return;

            int userId = int.Parse(userIdClaim.Value);

            // ✅ USER FEATURE CHECK
            bool hasUserFeature = await _context.UserFeatures
                .Include(x => x.Feature)
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Feature.Code == requirement.FeatureCode &&
                    x.IsEnabled);

            if (hasUserFeature)
            {
                context.Succeed(requirement);
                return;
            }

            // ✅ ROLE FEATURE CHECK
            var roles = context.User.FindAll(ClaimTypes.Role)
                .Select(r => r.Value)
                .ToList();

            bool hasRoleFeature = await _context.RoleFeatures
                .Include(x => x.Feature)
                .Include(x => x.Role)
                .AnyAsync(x =>
                    roles.Contains(x.Role.Name) &&
                    x.Feature.Code == requirement.FeatureCode &&
                    x.IsEnabled);

            if (hasRoleFeature)
                context.Succeed(requirement);
        }
    }
}
