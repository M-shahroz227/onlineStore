using Microsoft.AspNetCore.Authorization;
using onlineStore.Service.Interfaces;
using System.Security.Claims;

namespace onlineStore.Authorization
{
    public class FeatureAuthorizationHandler
        : AuthorizationHandler<FeatureRequirement>
    {
        private readonly IFeatureService _featureService;

        public FeatureAuthorizationHandler(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            FeatureRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return;

            int userId = int.Parse(userIdClaim.Value);

            bool hasFeature = await _featureService
                .HasFeatureAsync(userId, requirement.FeatureCode);

            if (hasFeature)
            {
                context.Succeed(requirement);
            }
        }
    }
}
