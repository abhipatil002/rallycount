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
                    CASE WHEN team1_score > team2_score THEN 1
                         WHEN team1_score < team2_score THEN -1 ELSE 0 END AS points
                FROM matches
                UNION ALL
                SELECT period_id, team1_p2,
                    CASE WHEN team1_score > team2_score THEN 1
                         WHEN team1_score < team2_score THEN -1 ELSE 0 END
                FROM matches
                UNION ALL
                SELECT period_id, team2_p1,
                    CASE WHEN team2_score > team1_score THEN 1
                         WHEN team2_score < team1_score THEN -1 ELSE 0 END
                FROM matches
                UNION ALL
                SELECT period_id, team2_p2,
                    CASE WHEN team2_score > team1_score THEN 1
                         WHEN team2_score < team1_score THEN -1 ELSE 0 END
                FROM matches
              )
              SELECT p.id, p.name, p.avatar_url,
                     COALESCE(SUM(pr.points), 0)                              AS score,
                     COUNT(pr.player_id) FILTER (WHERE pr.points = 1)         AS wins,
                     COUNT(pr.player_id) FILTER (WHERE pr.points = -1)        AS losses,
                     COUNT(pr.player_id)                                       AS matches_played
              FROM players p
              LEFT JOIN player_results pr ON p.id = pr.player_id AND pr.period_id = @periodId
              GROUP BY p.id, p.name, p.avatar_url
              ORDER BY score ASC, losses DESC",
            new { periodId });
    }
}
