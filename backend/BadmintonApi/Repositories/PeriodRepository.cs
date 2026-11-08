using Dapper;
using BadmintonApi.DTOs;
using BadmintonApi.Models;
using Npgsql;

namespace BadmintonApi.Repositories;

public class PeriodRepository(IConfiguration config)
{
    private NpgsqlConnection Conn() => new(config.GetConnectionString("DefaultConnection"));

    public async Task<Period?> GetActive()
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Period>(
            "SELECT * FROM periods WHERE is_active = TRUE");
    }

    public async Task<IEnumerable<Period>> GetAll()
    {
        using var conn = Conn();
        return await conn.QueryAsync<Period>("SELECT * FROM periods ORDER BY start_date DESC");
    }

    public async Task<Period?> GetById(Guid id)
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Period>("SELECT * FROM periods WHERE id = @id", new { id });
    }

    public async Task<Period> Create(CreatePeriodDto dto)
    {
        using var conn = Conn();
        return await conn.QuerySingleAsync<Period>(
            @"INSERT INTO periods (label, frequency, start_date)
              VALUES (@Label, @Frequency, @StartDate)
              RETURNING *", dto);
    }

    public async Task<Period?> Update(Guid id, UpdatePeriodDto dto)
    {
        using var conn = Conn();
        return await conn.QueryFirstOrDefaultAsync<Period>(
            @"UPDATE periods SET label = @Label, frequency = @Frequency,
                start_date = @StartDate, end_date = @EndDate
              WHERE id = @id RETURNING *",
            new { id, dto.Label, dto.Frequency, dto.StartDate, dto.EndDate });
    }

    public async Task Activate(Guid id)
    {
        using var conn = Conn();
        await conn.ExecuteAsync("UPDATE periods SET is_active = FALSE WHERE is_active = TRUE");
        await conn.ExecuteAsync("UPDATE periods SET is_active = TRUE WHERE id = @id", new { id });
    }
}
