namespace UniDocuments.Text.Domain.Services.DocumentMapping;

public interface IDocumentMapper
{
    string GetDocumentName(Guid documentId);
    Guid GetDocumentId(int id);
    void AddMap(Guid documentId, string documentName);
}