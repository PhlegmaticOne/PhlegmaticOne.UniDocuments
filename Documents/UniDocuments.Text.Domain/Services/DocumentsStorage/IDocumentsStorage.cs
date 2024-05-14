using System.Runtime.CompilerServices;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface IDocumentsStorage
{
    Task<DocumentLoadResponse> LoadAsync(Guid id, CancellationToken cancellationToken);
    ConfiguredCancelableAsyncEnumerable<DocumentLoadResponse> LoadAsync(IList<Guid> ids, CancellationToken cancellationToken);
    Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken);
}