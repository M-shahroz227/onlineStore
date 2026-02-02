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
            // Username already exists
            if (await _context.Users.AnyAsync(u => u.UserName == dto.Username))
                return false;

            var user = new User
            {
                UserName = dto.Username,
                PasswordHash = PasswordHelper.Hash(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 🔹 Assign default role (User)
            var userRole = await _context.Roles.FirstAsync(r => r.Name == dto.Username);
            _context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = userRole.Id
            });

            // 🔹 Assign features via role
            var roleFeatures = await _context.RoleFeatures
                .Where(rf => rf.RoleId == userRole.Id)
                .ToListAsync();

            foreach (var rf in roleFeatures)
            {
                _context.UserFeatures.Add(new UserFeature
                {
                    UserId = user.Id,
                    FeatureId = rf.FeatureId
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }


        // -------------------- LOGIN --------------------
        public async Task<string> LoginUserAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserFeatures)
                    .ThenInclude(uf => uf.Feature)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if (user == null)
                return null;

            // 🔐 Correct password check
            if (!PasswordHelper.Verify(dto.password, user.PasswordHash))
                return null;

            // Generate JWT
            var token = _jwtService.GenerateToken(user);
            return token;
        }

    }
}
