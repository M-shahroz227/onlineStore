namespace onlineStore.Model
{
    public class Feature
    {
        public int Id { get; set; }

        public required string Code { get; set; }
        public required string Description { get; set; } = string.Empty;

        public ICollection<RoleFeature> RoleFeatures { get; set; } = new List<RoleFeature>();
        public ICollection<UserFeature> UserFeatures { get; set; } = new List<UserFeature>();
    }
}
