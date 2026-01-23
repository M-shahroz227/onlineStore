using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using System;

namespace onlineStore.Service.FeatureService
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
            // 1️⃣ Direct user feature
            if (await _context.UserFeatures
                .AnyAsync(x => x.UserId == userId && x.Feature.Code == featureCode))
                return true;

            // 2️⃣ Role based feature
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .AnyAsync(ur =>
                    ur.Role.RoleFeatures.Any(rf => rf.Feature.Code == featureCode)
                );
        }
    }

}
