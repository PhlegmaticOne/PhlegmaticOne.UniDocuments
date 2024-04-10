using Microsoft.Data.SqlClient;
using UniDocuments.Text.Domain.Services.FileStorage;

namespace UniDocuments.Text.Services.FileStorage.Sql;

public class SqlConnectionProvider : ISqlConnectionProvider
{
    private readonly string _connectionString;

    public SqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Connection { get; private set; } = null!;
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        Connection = new SqlConnection(_connectionString);
        await Connection.OpenAsync(cancellationToken);
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