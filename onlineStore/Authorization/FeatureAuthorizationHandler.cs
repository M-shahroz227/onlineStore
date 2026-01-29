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
            var userIdClaim = context.User.FindFirst("UserId");
            if (userIdClaim == null)
                return;

            int userId = int.Parse(userIdClaim.Value);

            bool allowed = await _context.UserFeatures
           .AnyAsync(uf =>
             uf.UserId == userId &&
             uf.Feature.Code == requirement.Feature &&
             uf.Feature.IsActive);


            if (allowed)
            {
                context.Succeed(requirement);
            }
        }
    }
}
