using UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.Text.Services.Preprocessing.StopWords;

public class StopWordsLoaderFile : IStopWordsLoader
{
    private const string StopWordsFileName = "stopwords.txt";
    
    private readonly ISavePathProvider _savePathProvider;

    public StopWordsLoaderFile(ISavePathProvider savePathProvider)
    {
        _savePathProvider = savePathProvider;
    }
    
    public Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_savePathProvider.SavePath, StopWordsFileName);
        return File.ReadAllLinesAsync(filePath, cancellationToken);
    }
}