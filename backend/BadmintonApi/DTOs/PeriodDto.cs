namespace BadmintonApi.DTOs;

public record CreatePeriodDto(string Label, string Frequency, DateOnly StartDate);
public record UpdatePeriodDto(string Label, string Frequency, DateOnly StartDate, DateOnly? EndDate);
