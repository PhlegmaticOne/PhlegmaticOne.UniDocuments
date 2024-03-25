namespace UniDocuments.Text.Domain.Services.StreamReading;

public interface IStreamContentReader
{
    Task<string> ReadAsync(Stream stream, CancellationToken cancellationToken);
}