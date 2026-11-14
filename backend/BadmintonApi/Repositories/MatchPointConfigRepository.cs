using Dapper;
using Npgsql;

namespace BadmintonApi.Repositories;

public class MatchPointConfigRepository(IConfiguration config)
{
    private NpgsqlConnection Conn() => new(config.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<int>> GetAll()
    {
        using var conn = Conn();
        return await conn.QueryAsync<int>("SELECT value FROM match_point_configs ORDER BY value DESC");
    }

    public async Task<int> Create(int value)
    {
        using var conn = Conn();
        return await conn.QuerySingleAsync<int>(
            "INSERT INTO match_point_configs (value) VALUES (@value) ON CONFLICT (value) DO UPDATE SET value = EXCLUDED.value RETURNING value",
            new { value });
    }
}
