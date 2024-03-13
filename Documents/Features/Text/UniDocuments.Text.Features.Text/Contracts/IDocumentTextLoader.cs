namespace UniDocuments.Text.Features.Text.Contracts;

public interface IDocumentTextLoader
{
    Task<string> LoadTextAsync(Guid documentId);
}