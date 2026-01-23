using Microsoft.AspNetCore.Connections.Features;

namespace onlineStore.Model
{
    public class Feature
    {
        public int Id { get; set; }
        public string Code { get; set; } // e.g. PRODUCT_CREATE
        public string Description { get; set; }

        public ICollection<RoleFeature> RoleFeatures { get; set; }
        public ICollection<UserFeature> UserFeatures { get; set; }
    }

}
