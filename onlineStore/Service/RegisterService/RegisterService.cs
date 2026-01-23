using onlineStore.Data;
using onlineStore.DTO.RegisterDto;
using onlineStore.Model;
using onlineStore.Service.JwtService;

namespace onlineStore.Service.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private readonly StoreDbContext _context;
        private readonly IJwtService _jwtService;

        public RegisterService(StoreDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<RegisterDto> RegisterUser(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                throw new Exception("Username already exists");
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                PhoneNumber = dto.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Entity → DTO mapping
            return new RegisterDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber

            };
        }
    }
}
