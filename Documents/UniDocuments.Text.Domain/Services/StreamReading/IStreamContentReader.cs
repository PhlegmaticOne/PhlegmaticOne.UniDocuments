namespace UniDocuments.Text.Domain.Services.StreamReading;

public interface IStreamContentReader
{
    Task<UniDocumentContent> ReadAsync(Stream stream, CancellationToken cancellationToken);
}