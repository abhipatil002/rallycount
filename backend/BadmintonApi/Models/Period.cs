namespace BadmintonApi.Models;

public class Period
{
    public Guid Id { get; set; }
    public string Label { get; set; } = "";
    public string Frequency { get; set; } = "weekly";
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
