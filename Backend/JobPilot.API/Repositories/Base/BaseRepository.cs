using System.Data;
using Microsoft.Data.SqlClient;
using JobPilot.API.Data;

namespace JobPilot.API.Repositories.Base;

public abstract class BaseRepository
{
    protected readonly DbConnectionFactory _db;

    protected BaseRepository(DbConnectionFactory db)
    {
        _db = db;
    }

    protected SqlConnection CreateConnection()
    {
        return _db.CreateConnection();
    }

    protected SqlCommand CreateStoredProcedure(
        string procedureName,
        SqlConnection connection)
    {
        return new SqlCommand(procedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
    }

    protected void AddParameter(
        SqlCommand command,
        string name,
        object? value)
    {
        command.Parameters.AddWithValue(
            name,
            value ?? DBNull.Value);
    }
}