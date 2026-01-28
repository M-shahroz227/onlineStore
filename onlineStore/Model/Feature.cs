namespace onlineStore.Model
{
    public class Feature
    {
        public int Id { get; set; }

        public required string Code { get; set; }
        public required string Description { get; set; } = string.Empty;
        public bool IsActive { get; internal set; }
        public string Name { get; internal set; }
    }
}
