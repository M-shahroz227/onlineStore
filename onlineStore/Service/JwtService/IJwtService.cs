using onlineStore.Model;

namespace onlineStore.Service.JwtService
{
    public interface IJwtService
    {
        string GenerateToken(User register);
    }
}
