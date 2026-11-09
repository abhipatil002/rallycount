using Dapper;
using BadmintonApi.DTOs;
using Npgsql;

namespace BadmintonApi.Repositories;

public class LeaderboardRepository(IConfiguration config)
{
    private NpgsqlConnection Conn() => new(config.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<LeaderboardEntryDto>> GetByPeriod(Guid periodId)
    {
        using var conn = Conn();
        return await conn.QueryAsync<LeaderboardEntryDto>(
            @"WITH player_results AS (
                SELECT period_id, team1_p1 AS player_id,
                    team1_score - team2_score AS points,
                    CASE WHEN team1_score > team2_score THEN 1 ELSE 0 END AS is_win,
                    CASE WHEN team1_score < team2_score THEN 1 ELSE 0 END AS is_loss
                FROM matches WHERE deleted_at IS NULL
                UNION ALL
                SELECT period_id, team1_p2,
                    team1_score - team2_score,
                    CASE WHEN team1_score > team2_score THEN 1 ELSE 0 END,
                    CASE WHEN team1_score < team2_score THEN 1 ELSE 0 END
                FROM matches WHERE deleted_at IS NULL
                UNION ALL
                SELECT period_id, team2_p1,
                    team2_score - team1_score,
                    CASE WHEN team2_score > team1_score THEN 1 ELSE 0 END,
                    CASE WHEN team2_score < team1_score THEN 1 ELSE 0 END
                FROM matches WHERE deleted_at IS NULL
                UNION ALL
                SELECT period_id, team2_p2,
                    team2_score - team1_score,
                    CASE WHEN team2_score > team1_score THEN 1 ELSE 0 END,
                    CASE WHEN team2_score < team1_score THEN 1 ELSE 0 END
                FROM matches WHERE deleted_at IS NULL
              )
              SELECT p.id, p.name, p.avatar_url,
                     COALESCE(SUM(pr.points), 0)       AS score,
                     COALESCE(SUM(pr.is_win), 0)        AS wins,
                     COALESCE(SUM(pr.is_loss), 0)       AS losses,
                     COUNT(pr.player_id)                 AS matches_played
              FROM players p
              LEFT JOIN player_results pr ON p.id = pr.player_id AND pr.period_id = @periodId
              GROUP BY p.id, p.name, p.avatar_url
              ORDER BY score DESC, wins DESC",
            new { periodId });
    }
}
