using Microsoft.AspNetCore.Mvc;
using onlineStore.DTO.LoginDto;
using onlineStore.Service.LoginService;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<ActionResult> LoginUser([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _loginService.LoginAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
