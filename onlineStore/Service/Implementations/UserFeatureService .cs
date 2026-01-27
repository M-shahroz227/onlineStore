using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

namespace onlineStore.Service.Implementations
{
    public class UserFeatureService : IUserFeatureService
    {
        private readonly StoreDbContext _context;

        public UserFeatureService(StoreDbContext context)
        {
            _context = context;
        }

        public async Task AssignAsync(int userId, int featureId)
        {
            var exists = await _context.UserFeatures
                .AnyAsync(x => x.UserId == userId && x.FeatureId == featureId);

            if (exists)
                throw new Exception("Permission already assigned");

            _context.UserFeatures.Add(new UserFeature
            {
                UserId = userId,
                FeatureId = featureId
            });

            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(int userId, int featureId)
        {
            var uf = await _context.UserFeatures
                .FirstOrDefaultAsync(x => x.UserId == userId && x.FeatureId == featureId);

            if (uf == null)
                throw new Exception("Permission not found");

            _context.UserFeatures.Remove(uf);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Feature>> GetUserFeaturesAsync(int userId)
        {
            return await _context.UserFeatures
                .Include(x => x.Feature)
                .Where(x => x.UserId == userId)
                .Select(x => x.Feature)
                .ToListAsync();
        }
    }
}
