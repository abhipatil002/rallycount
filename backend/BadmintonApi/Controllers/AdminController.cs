using BadmintonApi.DTOs;
using BadmintonApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController(PeriodRepository periods, PlayerRepository players) : ControllerBase
{
    [HttpGet("periods")]
    public async Task<IActionResult> GetPeriods() => Ok(await periods.GetAll());

    [HttpPost("periods")]
    public async Task<IActionResult> CreatePeriod([FromBody] CreatePeriodDto dto) =>
        Ok(await periods.Create(dto));

    [HttpPut("periods/{id:guid}")]
    public async Task<IActionResult> UpdatePeriod(Guid id, [FromBody] UpdatePeriodDto dto)
    {
        var updated = await periods.Update(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPut("periods/{id:guid}/activate")]
    public async Task<IActionResult> ActivatePeriod(Guid id)
    {
        var period = await periods.GetById(id);
        if (period is null) return NotFound();
        await periods.Activate(id);
        return NoContent();
    }

    [HttpGet("players")]
    public async Task<IActionResult> GetPlayers() => Ok(await players.GetAll());

    [HttpPut("players/{id:guid}/toggle-admin")]
    public async Task<IActionResult> ToggleAdmin(Guid id)
    {
        var player = await players.GetById(id);
        if (player is null) return NotFound();
        await players.SetAdmin(id, !player.IsAdmin);
        return NoContent();
    }
}
