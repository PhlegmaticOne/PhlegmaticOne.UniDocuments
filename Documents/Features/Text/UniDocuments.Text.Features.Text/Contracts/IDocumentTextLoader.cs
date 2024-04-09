using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Features.Text.Contracts;

public interface IDocumentTextLoader
{
    Task<StreamContentReadResult> LoadTextAsync(Guid documentId, CancellationToken cancellationToken);
}