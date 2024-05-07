namespace UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;

public interface IStopWordsService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    bool IsStopWord(string word);
}