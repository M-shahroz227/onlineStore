using onlineStore.DTO.RegisterDto;
using onlineStore.Model;

namespace onlineStore.Service.RegisterService
{
    public interface IRegisterService
    {
        Task<RegisterDto> RegisterUser(RegisterDto dto);
    }
}
