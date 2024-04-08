using Microsoft.Data.SqlClient;

namespace UniDocuments.App.Services.FileStorage.Sql.Connection;

public interface ISqlConnectionProvider : IDisposable, IAsyncDisposable
{
    SqlConnection Connection { get; }
    Task InitializeAsync();
}