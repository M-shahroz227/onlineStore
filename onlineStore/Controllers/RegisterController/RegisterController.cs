using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onlineStore.Model;
using onlineStore.Service.RegisterService;

namespace onlineStore.Controllers.RegisterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        [HttpPost("register")]
        public  async Task<ActionResult<Register>> RegisterUser(Register register)
        {
            var newUser = await  _registerService.RegisterUser(register);
            return Ok(newUser);
        }
    }
}
