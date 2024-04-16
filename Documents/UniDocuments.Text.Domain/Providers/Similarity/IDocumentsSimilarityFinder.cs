using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Providers.Similarity.Responses;

namespace UniDocuments.Text.Domain.Providers.Similarity;

public interface IDocumentsSimilarityFinder
{
    Task<SimilarityResponse> CompareAsync(
        DocumentsSimilarityRequest request, CancellationToken cancellationToken);
    Task<List<SimilarityResponse>> CompareAsync(
        TextsSimilarityRequest request, CancellationToken cancellationToken);
}