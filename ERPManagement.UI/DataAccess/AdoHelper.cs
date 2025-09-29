using Microsoft.Data.SqlClient;
using System.Data;

public class AdoHelper
{
    private readonly string _connectionString;

    public AdoHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<DataTable> ExecuteQueryAsync(string sql, Dictionary<string, object>? parameters = null)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        if (parameters != null)
        {
            foreach (var param in parameters)
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
        }

        var dt = new DataTable();
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        dt.Load(reader);

        return dt;
    }
}
