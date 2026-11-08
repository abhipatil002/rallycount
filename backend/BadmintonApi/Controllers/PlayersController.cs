using System.Security.Claims;
using BadmintonApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/players")]
[Authorize]
public class PlayersController(PlayerRepository players) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await players.GetAll());

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var player = await players.GetById(id);
        return player is null ? NotFound() : Ok(player);
    }
}
