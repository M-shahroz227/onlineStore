namespace onlineStore.Model
{
    public class RoleFeature
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }

}
