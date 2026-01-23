namespace onlineStore.Service.Interfaces
{
    public interface IFeatureService
    {
        Task<bool> HasFeatureAsync(int userId, string featureCode);
    }
}
