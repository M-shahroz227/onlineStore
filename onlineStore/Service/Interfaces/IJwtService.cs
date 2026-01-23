using onlineStore.Model;

namespace onlineStore.Service.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
