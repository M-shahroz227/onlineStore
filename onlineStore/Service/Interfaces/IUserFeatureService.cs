using onlineStore.Model;

namespace onlineStore.Service.Interfaces
{
    public interface IUserFeatureService
    {
        Task AssignAsync(int userId, int featureId);
        Task RevokeAsync(int userId, int featureId);
        Task<List<Feature>> GetUserFeaturesAsync(int userId);
    }
}
