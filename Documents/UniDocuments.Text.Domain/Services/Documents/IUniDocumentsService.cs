namespace UniDocuments.Text.Domain.Services.Documents;

public interface IUniDocumentsService
{
    Task<UniDocument> GetAsync(Guid id, CancellationToken cancellationToken);
    Task SaveAsync(UniDocument document, CancellationToken cancellationToken);
}