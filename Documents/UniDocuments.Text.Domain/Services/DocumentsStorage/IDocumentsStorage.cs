using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface IDocumentsStorage
{
    Task<IDocumentLoadResponse?> LoadAsync(Guid id, CancellationToken cancellationToken);
    IAsyncEnumerable<IDocumentLoadResponse> LoadAsync(IList<Guid> ids, CancellationToken cancellationToken);
    Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken);
}