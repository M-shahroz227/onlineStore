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

        public async Task<Feature> CreateAsync(Feature feature)
        {
            var exists = await _context.Features
                .AnyAsync(f => f.Code == feature.Code);

            if (exists)
                throw new Exception("Feature already exists");

            _context.Features.Add(feature);
            await _context.SaveChangesAsync();

            return feature;
        }


        public async Task ToggleAsync(int featureId)
        {
            var feature = await _context.Features.FindAsync(featureId);
            if (feature == null)
                throw new Exception("Feature not found");

            
            await _context.SaveChangesAsync();
        }
    }
}
