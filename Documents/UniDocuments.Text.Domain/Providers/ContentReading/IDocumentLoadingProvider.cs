namespace UniDocuments.Text.Domain.Providers.ContentReading;

public interface IDocumentLoadingProvider
{
    Task<UniDocument?> LoadAsync(Guid documentId, bool cache, CancellationToken cancellationToken);
    Task<Dictionary<Guid, UniDocument>> LoadAsync(ISet<Guid> documentId, bool cache, CancellationToken cancellationToken);
}