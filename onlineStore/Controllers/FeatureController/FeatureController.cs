using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onlineStore.Model;
using onlineStore.Service.Interfaces;

[ApiController]
[Route("api/features")]

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

    
}
