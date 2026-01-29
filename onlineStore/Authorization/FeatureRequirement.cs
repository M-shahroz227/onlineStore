using Microsoft.AspNetCore.Authorization;

namespace onlineStore.Authorization
{
    public class FeatureRequirement : IAuthorizationRequirement
    {
        
        public string Feature { get; internal set; }

        public FeatureRequirement(string featureCode)
        {
            Feature = featureCode;
        }
    }
}
