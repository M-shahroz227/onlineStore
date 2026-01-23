using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
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

        public async Task<bool> HasFeatureAsync(int userId, string featureCode)
        {
            // 1️⃣ Direct User Feature
            bool userFeature = await _context.UserFeatures
                .Include(x => x.Feature)
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Feature.Code == featureCode);

            if (userFeature)
                return true;

            // 2️⃣ Role Based Feature
            return await _context.UserRoles
                .Include(x => x.Role)
                    .ThenInclude(r => r.RoleFeatures)
                        .ThenInclude(rf => rf.Feature)
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Role.RoleFeatures
                        .Any(rf => rf.Feature.Code == featureCode));
        }
    }
}
