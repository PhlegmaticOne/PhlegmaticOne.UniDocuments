namespace UniDocuments.Text.Domain.Providers.ContentReading;

public interface IDocumentsProvider
{
    Task<Dictionary<Guid, UniDocument>> GetDocumentsAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken);
}