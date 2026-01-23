using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.DTO;
using onlineStore.DTO.LoginDto;
using onlineStore.DTO.RegisterDto;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

namespace onlineStore.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly StoreDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(StoreDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // -------------------- REGISTER --------------------
        public async Task<bool> RegisterUserAsync(RegisterDto dto)
        {
            // Check if username exists
            if (await _context.Users.AnyAsync(u => u.UserName == dto.Username))
                return false;

            var user = new User
            {
                UserName = dto.Username,
                PasswordHash = dto.Password // Hash in real app
            };

            // Add features
            var features = await _context.Features.ToListAsync();
            foreach (var feature in features)
            {
                user.UserFeatures.Add(new UserFeature { Feature = feature});
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // -------------------- LOGIN --------------------
        public async Task<string> LoginUserAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserFeatures)
                .FirstOrDefaultAsync(u => u.UserName == dto.Username && u.PasswordHash == dto.password);

            if (user == null) return null;

            // Extract feature names
            var features = user.UserFeatures.Select(f => f.Feature).ToList();

            // Generate JWT
            var token = _jwtService.GenerateToken(user);

            return token;
        }
    }
}
