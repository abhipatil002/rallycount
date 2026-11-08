namespace BadmintonApi.Models;

public class Player
{
    public Guid Id { get; set; }
    public string GoogleId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? AvatarUrl { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
}
