namespace UniDocuments.Text.Domain.Services.Reading;

public interface IStreamContentReader
{
    Task<string> ReadAsync(Stream stream, CancellationToken cancellationToken);
}