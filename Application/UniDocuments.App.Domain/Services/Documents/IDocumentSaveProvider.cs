using UniDocuments.Text.Domain;

namespace UniDocuments.App.Domain.Services.Documents;

public interface IDocumentSaveProvider
{
    Task<UniDocument> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken);
}