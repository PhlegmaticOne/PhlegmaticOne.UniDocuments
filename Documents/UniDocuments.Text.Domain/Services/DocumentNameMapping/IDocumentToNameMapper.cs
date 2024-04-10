namespace UniDocuments.Text.Domain.Services.DocumentNameMapping;

public interface IDocumentToNameMapper
{
    Task InitializeAsync(CancellationToken cancellationToken);
    string GetDocumentName(Guid documentId);
    void AddMap(Guid documentId, string documentName);
}