using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.DTO.LoginDto;
using onlineStore.Service.JwtService;
using onlineStore.Service.LoginService;

public class LoginService : ILoginService
{
    private readonly StoreDbContext _context;
    private readonly IJwtService _jwtService;

    public LoginService(StoreDbContext context, IJwtService jwtService) 
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        // 1️⃣ Find user by username
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null)
            throw new Exception("Invalid username or password");

        // 2️⃣ Verify password using BCrypt
        bool isValid = BCrypt.Net.BCrypt.Verify(dto.password, user.Password);

        if (!isValid)
            throw new Exception("Invalid username or password");

        // 3️⃣ Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return (token);
    }
}
