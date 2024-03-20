namespace UniDocuments.Text.Domain.Services.Processing;

public interface IStopWordsService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    bool IsStopWord(string word);
}