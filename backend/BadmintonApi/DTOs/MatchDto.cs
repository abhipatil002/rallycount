namespace BadmintonApi.DTOs;

public record CreateMatchDto(
    Guid PeriodId,
    Guid Team1P1,
    Guid Team1P2,
    Guid Team2P1,
    Guid Team2P2,
    int Team1Score,
    int Team2Score
);
