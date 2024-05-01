using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppInitializer
{
    public static async Task InitializeAsync(WebApplication application, CancellationToken cancellationToken)
    {
        var useRealDatabase = application.Configuration.GetValue<bool>("UseRealDatabase");
        
        using var scope = application.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        var stopWordsService = services.GetRequiredService<IStopWordsService>();
        var documentMapperInitializer = services.GetRequiredService<IDocumentMappingInitializer>();
        var pythonTaskPool = services.GetRequiredService<IPythonTaskPool>();
        
        if (useRealDatabase)
        {
            var sqlConnectionProvider = services.GetRequiredService<ISqlConnectionProvider>();
            await sqlConnectionProvider.InitializeAsync(cancellationToken);
        }
        
        await stopWordsService.InitializeAsync(cancellationToken);
        await documentMapperInitializer.InitializeAsync(cancellationToken);
        pythonTaskPool.Start(cancellationToken);
    }
}