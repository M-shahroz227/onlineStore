using Microsoft.AspNetCore.Authorization;

namespace onlineStore.Authorization
{
    public class FeatureAuthorizeAttribute : AuthorizeAttribute
    {
        public FeatureAuthorizeAttribute(string featureCode)
        {
            Policy = $"Feature.{featureCode}"; // ✅ FIX

        }
    }
}
