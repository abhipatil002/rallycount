using BadmintonApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/periods")]
[Authorize]
public class PeriodsController(PeriodRepository periods) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await periods.GetAll());

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var period = await periods.GetActive();
        return period is null ? NotFound() : Ok(period);
    }
}
