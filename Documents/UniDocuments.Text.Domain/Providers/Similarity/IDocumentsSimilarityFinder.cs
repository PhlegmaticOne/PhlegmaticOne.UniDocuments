using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Providers.Similarity.Responses;

namespace UniDocuments.Text.Domain.Providers.Similarity;

public interface IDocumentsSimilarityFinder
{
    Task<DocumentsSimilarityResponse> CompareAsync(
        DocumentsSimilarityRequest request, CancellationToken cancellationToken);
}