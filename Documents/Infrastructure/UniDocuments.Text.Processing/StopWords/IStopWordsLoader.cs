namespace UniDocuments.Text.Processing.StopWords;

public interface IStopWordsLoader
{
    Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken);
}