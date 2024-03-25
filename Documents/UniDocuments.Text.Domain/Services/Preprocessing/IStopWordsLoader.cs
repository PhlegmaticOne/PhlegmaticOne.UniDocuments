namespace UniDocuments.Text.Domain.Services.Preprocessing;

public interface IStopWordsLoader
{
    Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken);
}