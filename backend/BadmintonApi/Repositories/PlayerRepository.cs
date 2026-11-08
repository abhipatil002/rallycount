using Dapper;
using BadmintonApi.Models;
using Npgsql;

namespace BadmintonApi.Repositories;

public class PlayerRepository(IConfiguration config)
{
    private NpgsqlConnection Conn() => new(config.GetConnectionString("DefaultConnection"));

    public async Task<Player?> GetByGoogleId(string googleId)
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Player>(
            "SELECT * FROM players WHERE google_id = @googleId", new { googleId });
    }

    public async Task<Player?> GetById(Guid id)
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Player>(
            "SELECT * FROM players WHERE id = @id", new { id });
    }

    public async Task<IEnumerable<Player>> GetAll()
    {
        using var conn = Conn();
        return await conn.QueryAsync<Player>("SELECT * FROM players ORDER BY name");
    }

    public async Task<Player> Upsert(string googleId, string name, string email, string? avatarUrl)
    {
        using var conn = Conn();
        return await conn.QuerySingleAsync<Player>(
            @"INSERT INTO players (google_id, name, email, avatar_url)
              VALUES (@googleId, @name, @email, @avatarUrl)
              ON CONFLICT (google_id) DO UPDATE
                SET name = EXCLUDED.name, avatar_url = EXCLUDED.avatar_url
              RETURNING *",
            new { googleId, name, email, avatarUrl });
    }

    public async Task SetAdmin(Guid id, bool isAdmin)
    {
        using var conn = Conn();
        await conn.ExecuteAsync("UPDATE players SET is_admin = @isAdmin WHERE id = @id", new { id, isAdmin });
    }
}
