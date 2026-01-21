using onlineStore.Model;

namespace onlineStore.Service.RegisterService
{
    public interface IRegisterService
    {
        Task<Register> RegisterUser(Register register);
    }
}
