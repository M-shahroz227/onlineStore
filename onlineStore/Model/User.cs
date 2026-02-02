using onlineStore.Model;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Feature> features { get; set; }  
    public ICollection<UserFeature> UserFeatures { get; set; }
}
