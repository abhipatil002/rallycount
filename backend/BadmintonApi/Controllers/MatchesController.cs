using System.Security.Claims;
using BadmintonApi.DTOs;
using BadmintonApi.Hubs;
using BadmintonApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/matches")]
[Authorize]
public class MatchesController(
    MatchRepository matches,
    LeaderboardRepository leaderboard,
    IHubContext<BadmintonHub> hub) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByPeriod([FromQuery] Guid periodId) =>
        Ok(await matches.GetByPeriod(periodId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMatchDto dto)
    {
        var submittedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var match = await matches.Create(dto, submittedBy);

        var board = await leaderboard.GetByPeriod(dto.PeriodId);
        await hub.Clients.Group(dto.PeriodId.ToString()).SendAsync("LeaderboardUpdated", board);

        return CreatedAtAction(nameof(GetByPeriod), new { periodId = dto.PeriodId }, match);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var match = await matches.GetById(id);
        if (match is null) return NotFound();
        var deletedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await matches.Delete(id, deletedBy);

        var board = await leaderboard.GetByPeriod(match.PeriodId);
        await hub.Clients.Group(match.PeriodId.ToString()).SendAsync("LeaderboardUpdated", board);

        return NoContent();
    }
}
