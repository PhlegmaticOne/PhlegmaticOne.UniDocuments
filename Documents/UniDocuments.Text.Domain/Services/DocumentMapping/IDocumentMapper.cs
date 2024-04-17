namespace UniDocuments.Text.Domain.Services.DocumentMapping;

public interface IDocumentMapper
{
    Task InitializeAsync(CancellationToken cancellationToken);
    string GetDocumentName(Guid documentId);
    Guid GetDocumentId(int id);
    void AddMap(Guid documentId, string documentName);
}