using onlineStore.DTO;
using onlineStore.DTO.LoginDto;
using onlineStore.DTO.RegisterDto;

namespace onlineStore.Service.AuthService
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterDto dto);
        Task<string> LoginUserAsync(LoginDto dto); // returns JWT token
    }
}
