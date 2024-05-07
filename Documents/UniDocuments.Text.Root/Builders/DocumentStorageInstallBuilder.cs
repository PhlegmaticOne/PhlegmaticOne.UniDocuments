using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Services.FileStorage.Sql;

namespace UniDocuments.Text.Root.Builders;

public class DocumentStorageInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentStorageInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseSqlConnectionString(string connectionString)
    {
        _serviceCollection.AddSingleton<ISqlConnection, SqlConnection>(_ =>
        {
            return new SqlConnection(connectionString);
        });
    }
}