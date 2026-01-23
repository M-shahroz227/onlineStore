using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineStore.Data;
using onlineStore.DTO.LoginDto;
using onlineStore.Service.Interfaces;

namespace onlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {
        private readonly StoreDbContext _context;
        private readonly IJwtService _jwtService;
        public AuthController(StoreDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserFeatures)
                .FirstOrDefaultAsync(u => u.UserName == dto.Username && u.PasswordHash == dto.password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            user.UserFeatures = (ICollection<Model.UserFeature>)user.UserFeatures.Select(f => f.Feature).ToList();

            var token = _jwtService.GenerateToken(user);

            return Ok(new { Token = token });
        }

    }
}
