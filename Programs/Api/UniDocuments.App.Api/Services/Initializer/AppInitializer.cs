using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.App.Api.Services.Initializer;

public class AppInitializer : IAppInitializer
{
    private readonly IStopWordsLoader _stopWordsLoader;

    public AppInitializer(IStopWordsLoader stopWordsLoader)
    {
        _stopWordsLoader = stopWordsLoader;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _stopWordsLoader.LoadStopWordsAsync(cancellationToken);
    }
}