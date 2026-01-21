using onlineStore.DTO.LoginDto;

namespace onlineStore.Service.LoginService
{
    public interface ILoginService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
