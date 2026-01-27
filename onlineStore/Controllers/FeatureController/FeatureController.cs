using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

[ApiController]
[Route("api/features")]
[Authorize(Roles = "Manager")]
public class FeatureController : ControllerBase
{
    private readonly IFeatureService _service;

    public FeatureController(IFeatureService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Feature feature)
        => Ok(await _service.CreateAsync(feature));

    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> Toggle(int id)
    {
        await _service.ToggleAsync(id);
        return Ok();
    }
}
