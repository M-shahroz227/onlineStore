using Microsoft.AspNetCore.Authorization;

namespace onlineStore.Authorization
{
    public class FeatureRequirement : IAuthorizationRequirement
    {
        public string FeatureCode { get; }

        public FeatureRequirement(string featureCode)
        {
            FeatureCode = featureCode;
        }
    }
}
