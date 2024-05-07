using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface ISqlConnection : IAsyncDisposable
{
    SqlConnection Connection { get; }
    Task InitializeAsync(CancellationToken cancellationToken);
}