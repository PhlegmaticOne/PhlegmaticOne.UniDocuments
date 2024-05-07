using UniDocuments.Text.Domain.Services.DocumentsStorage;

namespace UniDocuments.Text.Services.FileStorage.Sql;

public class SqlConnection : ISqlConnection
{
    private readonly string _connectionString;

    public SqlConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Microsoft.Data.SqlClient.SqlConnection Connection { get; private set; } = null!;
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        Connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        await Connection.OpenAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return Connection.DisposeAsync();
    }
}