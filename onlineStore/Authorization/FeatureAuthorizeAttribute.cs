using Microsoft.AspNetCore.Authorization;

public class FeatureAuthorizeAttribute : AuthorizeAttribute
{
    public FeatureAuthorizeAttribute(string featureCode)
    {
        Policy = $"Feature.{featureCode}";
    }
}
