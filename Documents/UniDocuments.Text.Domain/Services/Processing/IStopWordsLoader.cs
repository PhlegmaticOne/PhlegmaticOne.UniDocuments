namespace UniDocuments.Text.Domain.Services.Processing;

public interface IStopWordsLoader
{
    Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken);
}