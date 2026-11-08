using BadmintonApi.DTOs;
using BadmintonApi.Repositories;
using BadmintonApi.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(PlayerRepository players, TokenService tokens, IConfiguration config) : ControllerBase
{
    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleAuthDto dto)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [config["Google:ClientId"]]
                });
        }
        catch
        {
            return Unauthorized(new { message = "Invalid Google token" });
        }

        var player = await players.Upsert(payload.Subject, payload.Name, payload.Email, payload.Picture);
        var token = tokens.Generate(player);
        return Ok(new { token, player });
    }
}
