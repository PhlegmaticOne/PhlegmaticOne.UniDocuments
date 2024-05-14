namespace UniDocuments.Text.Domain.Services.StreamReading;

public interface IStreamContentReader
{
    UniDocumentContent Read(Stream stream);
    Task<UniDocumentContent> ReadAsync(Stream stream, CancellationToken cancellationToken);
}