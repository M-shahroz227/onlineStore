using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

namespace onlineStore.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly StoreDbContext _context;

        public UserService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
