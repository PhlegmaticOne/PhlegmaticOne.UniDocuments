using UniDocuments.Text.Domain.Services.Searching.Response;

namespace UniDocuments.Text.Features.Fingerprint.Services;

public interface IFingerprintSearcher
{
    Task<List<DocumentSearchData>> SearchTopAsync(Guid documentId, int topN);
}