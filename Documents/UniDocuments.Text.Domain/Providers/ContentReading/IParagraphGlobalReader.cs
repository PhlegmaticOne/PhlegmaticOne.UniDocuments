namespace UniDocuments.Text.Domain.Providers.ContentReading;

public interface IParagraphGlobalReader
{
    Task<string> ReadAsync(int globalParagraphId, CancellationToken cancellationToken);
}