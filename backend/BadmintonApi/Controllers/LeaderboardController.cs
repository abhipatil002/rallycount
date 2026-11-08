using BadmintonApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/leaderboard")]
[Authorize]
public class LeaderboardController(LeaderboardRepository leaderboard) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid periodId) =>
        Ok(await leaderboard.GetByPeriod(periodId));
}
