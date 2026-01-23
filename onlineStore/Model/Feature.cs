namespace onlineStore.Model
{
    public class Feature
    {
        public int Id { get; set; }

        // Required non-nullable string
        public required string Code { get; set; } // e.g. PRODUCT_CREATE or PRODUCT_DELETE
        public required string Description { get; set; }= string.Empty;

        // Initialize collections to avoid null reference
        public ICollection<RoleFeature> RoleFeatures { get; set; } = new List<RoleFeature>();
        public ICollection<UserFeature> UserFeatures { get; set; } = new List<UserFeature>();
    }
}
