namespace onlineStore.DTO.RegisterDto
{
    public class RegisterDto
    {
        public int Id { get; set; }

        // Mark as required to satisfy C# 11 non-nullable rules
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }          // optional
        public string? PhoneNumber { get; set; }    // optional
        public string Role { get; set; } = "User";  // default role

        // Add Features list for assigning feature-based authorization
        public List<string> Features { get; set; } = new List<string>();
    }
}
