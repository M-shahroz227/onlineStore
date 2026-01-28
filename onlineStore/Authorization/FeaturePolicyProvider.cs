using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace onlineStore.Authorization
{
    public class FeaturePolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        public FeaturePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _fallbackPolicyProvider.GetDefaultPolicyAsync();
        }
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Create a policy based on the feature code (policyName)
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new FeatureRequirement(policyName))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }
}
