using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BadmintonApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BadmintonApi.Services;

public class TokenService(IConfiguration config)
{
    public string Generate(Player player)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, player.Id.ToString()),
            new Claim(ClaimTypes.Name, player.Name),
            new Claim(ClaimTypes.Email, player.Email),
            new Claim("is_admin", player.IsAdmin.ToString().ToLower())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(int.Parse(config["Jwt:ExpiryDays"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
