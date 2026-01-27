using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onlineStore.Service.Interfaces;

[ApiController]
[Route("api/user-features")]
[Authorize(Roles = "Manager")]
public class UserFeatureController : ControllerBase
{
    private readonly IUserFeatureService _service;

    public UserFeatureController(IUserFeatureService service)
    {
        _service = service;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign(int userId, int featureId)
    {
        await _service.AssignAsync(userId, featureId);
        return Ok();
    }

    [HttpDelete("revoke")]
    public async Task<IActionResult> Revoke(int userId, int featureId)
    {
        await _service.RevokeAsync(userId, featureId);
        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFeatures(int userId)
        => Ok(await _service.GetUserFeaturesAsync(userId));
}
