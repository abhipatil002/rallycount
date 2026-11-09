namespace BadmintonApi.DTOs;

public class LeaderboardEntryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? AvatarUrl { get; set; }
    public long Score { get; set; }
    public long Nrr { get; set; }
    public long Wins { get; set; }
    public long Losses { get; set; }
    public long MatchesPlayed { get; set; }
}
