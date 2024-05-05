using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface IDocumentsStorage
{
    Task<DocumentLoadResponse> LoadAsync(DocumentLoadRequest loadRequest, CancellationToken cancellationToken);
    Task<Guid> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken);
}