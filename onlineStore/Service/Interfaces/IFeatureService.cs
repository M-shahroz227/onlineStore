using onlineStore.Model;

namespace onlineStore.Service.Interfaces
{
    public interface IFeatureService
    {
        Task<List<Feature>> GetAllAsync();
        Task<Feature> CreateAsync(Feature feature);
        Task ToggleAsync(int featureId);
    }
}
