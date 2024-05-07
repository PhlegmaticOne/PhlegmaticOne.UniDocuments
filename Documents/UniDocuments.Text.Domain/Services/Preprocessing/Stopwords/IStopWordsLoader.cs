namespace UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;

public interface IStopWordsLoader
{
    Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken);
}