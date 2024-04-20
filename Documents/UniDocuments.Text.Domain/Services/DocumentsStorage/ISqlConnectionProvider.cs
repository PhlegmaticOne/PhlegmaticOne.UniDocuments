using Microsoft.Data.SqlClient;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface ISqlConnectionProvider
{
    SqlConnection Connection { get; }
    Task InitializeAsync(CancellationToken cancellationToken);
}