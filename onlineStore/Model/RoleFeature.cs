namespace onlineStore.Model
{
    public class RoleFeature
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int FeatureId { get; set; }
        public Feature Feature { get; set; } = null!;

        public bool IsEnabled { get; set; } = true;
    }
}