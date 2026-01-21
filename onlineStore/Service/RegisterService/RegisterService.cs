using Microsoft.AspNetCore.Http.HttpResults;
using onlineStore.Data;
using onlineStore.Model;

namespace onlineStore.Service.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private  readonly StoreDbContext _context;
        public RegisterService(StoreDbContext context)
        {
            _context = context;
        }
        public async Task<Register> RegisterUser(Register register)
        {
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

        }
    }
}
