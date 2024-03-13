namespace UniDocuments.Text.Core.Services;

public interface IUniDocumentsService
{
    Task<UniDocument> GetDocumentAsync(Guid id);
}