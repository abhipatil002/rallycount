using Dapper;
using BadmintonApi.DTOs;
using BadmintonApi.Models;
using Npgsql;

namespace BadmintonApi.Repositories;

public class MatchRepository(IConfiguration config)
{
    private NpgsqlConnection Conn() => new(config.GetConnectionString("DefaultConnection"));

    public async Task<Match> Create(CreateMatchDto dto, Guid submittedBy)
    {
        using var conn = Conn();
        return await conn.QuerySingleAsync<Match>(
            @"INSERT INTO matches (period_id, team1_p1, team1_p2, team2_p1, team2_p2, team1_score, team2_score, submitted_by)
              VALUES (@PeriodId, @Team1P1, @Team1P2, @Team2P1, @Team2P2, @Team1Score, @Team2Score, @submittedBy)
              RETURNING *",
            new { dto.PeriodId, dto.Team1P1, dto.Team1P2, dto.Team2P1, dto.Team2P2, dto.Team1Score, dto.Team2Score, submittedBy });
    }

    public async Task<IEnumerable<Match>> GetByPeriod(Guid periodId)
    {
        using var conn = Conn();
        return await conn.QueryAsync<Match>(
            "SELECT * FROM matches WHERE period_id = @periodId ORDER BY played_at DESC", new { periodId });
    }

    public async Task<Match?> GetById(Guid id)
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Match>("SELECT * FROM matches WHERE id = @id", new { id });
    }

    public async Task Delete(Guid id, Guid deletedBy)
    {
        using var conn = Conn();
        await conn.ExecuteAsync(
            "UPDATE matches SET deleted_at = NOW(), deleted_by = @deletedBy WHERE id = @id",
            new { id, deletedBy });
    }
}
