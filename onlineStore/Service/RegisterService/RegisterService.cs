using Microsoft.AspNetCore.Http.HttpResults;
using onlineStore.Data;
using onlineStore.Model;
using onlineStore.Service.JwtService;

namespace onlineStore.Service.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private  readonly StoreDbContext _context;
        private readonly IJwtService _jwtService;
        public RegisterService(StoreDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<Register> RegisterUser(Register register)
        {
            if (_context.registers.Any(u => u.Username == register.Username))
            {
                throw new Exception("Username already exists");
            }
            var newUser = new Register
            {
                Username = register.Username,
                Password = register.Password,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber
            };
            _context.registers.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
            var token = _jwtService.GenerateToken(newUser);

            return newUser;

        }
    }
}
