using onlineStore.Model;

namespace onlineStore.Service.Interfaces
{
    public interface IFeatureService
    {
        Task<List<Feature>> GetAllAsync();
        
    }
}
