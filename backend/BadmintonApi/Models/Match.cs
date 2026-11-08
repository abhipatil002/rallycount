namespace BadmintonApi.Models;

public class Match
{
    public Guid Id { get; set; }
    public Guid PeriodId { get; set; }
    public Guid Team1P1 { get; set; }
    public Guid Team1P2 { get; set; }
    public Guid Team2P1 { get; set; }
    public Guid Team2P2 { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public DateTime PlayedAt { get; set; }
    public Guid SubmittedBy { get; set; }
}
