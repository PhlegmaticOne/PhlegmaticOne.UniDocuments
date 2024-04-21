using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintSearcher
{
    Task<DocumentSearchData[]> SearchTopAsync(Guid documentId, int topN, CancellationToken cancellationToken);
}