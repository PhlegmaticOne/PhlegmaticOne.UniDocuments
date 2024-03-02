namespace UniDocuments.Text.Processing.StopWords;

public interface IStopWordsService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    bool IsStopWord(string word);
}