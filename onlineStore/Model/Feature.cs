namespace onlineStore.Model
{
    public class Feature
    {
        public int Id { get; set; }
        public string Code { get; set; }  

        public string Name { get; set; }   

        public bool IsActive { get; set; }

        public ICollection<UserFeature> UserFeatures { get; set; }
        public ICollection<RoleFeature> RoleFeatures { get; set; } = new List<RoleFeature>();
    }
}
