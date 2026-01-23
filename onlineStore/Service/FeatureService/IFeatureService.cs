namespace onlineStore.Service.FeatureService
{
    public interface IFeatureService
    {
        Task<bool> HasFeatureAsync(int userId, string featureCode);
    }
}
