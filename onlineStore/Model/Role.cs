namespace onlineStore.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // ✅ REQUIRED
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RoleFeature> RoleFeatures { get; set; } = new List<RoleFeature>();
    }
}
