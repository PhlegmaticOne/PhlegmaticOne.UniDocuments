namespace UniDocuments.Text.Domain.Services.StreamReading;

public interface IStreamContentReader
{
    Task<StreamContentReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken);
}