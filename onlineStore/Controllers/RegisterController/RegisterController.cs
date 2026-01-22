using Microsoft.AspNetCore.Mvc;
using onlineStore.DTO.RegisterDto;
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

        // POST: api/Register/register
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _registerService.RegisterUser(dto);

            if (result == null)
                return BadRequest(new { message = "User already exists" });

            return Created("", result); // 201
        }
    }
}
