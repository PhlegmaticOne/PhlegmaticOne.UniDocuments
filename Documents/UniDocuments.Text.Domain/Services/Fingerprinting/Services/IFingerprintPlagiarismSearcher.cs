using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Services.Fingerprinting.Services;

public interface IFingerprintPlagiarismSearcher
{
    Task<DocumentSearchData[]> SearchTopAsync(Guid documentId, int topN, CancellationToken cancellationToken);
}