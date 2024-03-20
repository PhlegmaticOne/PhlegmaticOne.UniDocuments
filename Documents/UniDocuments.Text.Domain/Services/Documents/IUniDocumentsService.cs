namespace UniDocuments.Text.Domain.Services.Documents;

public interface IUniDocumentsService
{
    Task<UniDocument> GetDocumentAsync(Guid id);
}