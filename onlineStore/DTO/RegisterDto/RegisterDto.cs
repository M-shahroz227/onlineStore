namespace onlineStore.DTO.RegisterDto
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }          
        public string? PhoneNumber { get; set; }    
        public string Role { get; set; } = "User";  

        // Add Features list for assigning feature-based authorization
        public List<string> Features { get; set; } = new List<string>();
    }
}
