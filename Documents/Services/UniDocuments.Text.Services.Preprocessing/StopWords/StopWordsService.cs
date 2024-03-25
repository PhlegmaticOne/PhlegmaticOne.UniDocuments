using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.Text.Services.Preprocessing.StopWords;

public class StopWordsService : IStopWordsService
{
    private readonly IStopWordsLoader _stopWordsLoader;

    private HashSet<string>? _stopWords;

    public StopWordsService(IStopWordsLoader stopWordsLoader)
    {
        _stopWordsLoader = stopWordsLoader;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var stopWords = await _stopWordsLoader.LoadStopWordsAsync(cancellationToken);
        _stopWords = stopWords.ToHashSet(new StringComparerOrdinalIgnoreCase());
    }

    public bool IsStopWord(string word)
    {
        return _stopWords is not null && _stopWords.Contains(word);
    }
}