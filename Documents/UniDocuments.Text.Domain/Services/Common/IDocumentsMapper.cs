namespace UniDocuments.Text.Domain.Services.Common;

public interface IDocumentsMapper : IDocumentService
{
    Task InitializeAsync();
    string GetDocumentName(Guid documentId);
    void AddMap(Guid documentId, string documentName);
}