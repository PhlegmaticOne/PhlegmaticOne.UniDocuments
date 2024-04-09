using UniDocuments.Text.Domain.Services.Similarity.Request;
using UniDocuments.Text.Domain.Services.Similarity.Response;

namespace UniDocuments.Text.Domain.Services.Similarity;

public interface IDocumentSimilarityService : IDocumentService
{
    Task<UniDocumentsCompareResponse> CompareAsync(
        UniDocumentsCompareRequest request, CancellationToken cancellationToken);
}