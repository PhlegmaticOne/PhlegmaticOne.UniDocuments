namespace UniDocuments.Text.Domain.Services.Preprocessing;

public interface IStopWordsService
{
    Task InitializeAsync(CancellationToken cancellationToken);
    bool IsStopWord(string word);
}