using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Services.FileStorage.Connection;

namespace UniDocuments.Text.Root.Builders;

public class DocumentStorageInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public DocumentStorageInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void AddSqlConnectionFactory(string sqlConnection)
    {
        _serviceCollection.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(sqlConnection));
    }
}