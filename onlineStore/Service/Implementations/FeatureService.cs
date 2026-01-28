using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

namespace onlineStore.Service.Implementations
{
    public class FeatureService : IFeatureService
    {
        private readonly StoreDbContext _context;

        public FeatureService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Feature>> GetAllAsync()
        {
            return await _context.Features.ToListAsync();
        }
        
    }
}
