using Microsoft.Data.SqlClient;

namespace UniDocuments.App.Services.FileStorage.Sql.Connection;

public class SqlConnectionProvider : ISqlConnectionProvider
{
    private readonly string _connectionString;

    public SqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Connection { get; private set; } = null!;
    public async Task InitializeAsync()
    {
        Connection = new SqlConnection(_connectionString);
        await Connection.OpenAsync();
    }

    public void Dispose()
    {
        Connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Connection.DisposeAsync();
    }
}