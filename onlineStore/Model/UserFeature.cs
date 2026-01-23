namespace onlineStore.Model
{
    public class UserFeature
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int FeatureId { get; set; }
        public Feature Feature { get; set; } = null!;
       
    }
}
