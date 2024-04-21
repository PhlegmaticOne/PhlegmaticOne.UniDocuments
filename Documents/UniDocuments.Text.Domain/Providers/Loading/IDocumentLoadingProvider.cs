namespace UniDocuments.Text.Domain.Providers.Loading;

public interface IDocumentLoadingProvider
{
    Task<UniDocument> LoadAsync(Guid documentId, bool cache, CancellationToken cancellationToken);
}