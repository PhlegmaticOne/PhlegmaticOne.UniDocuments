using UniDocuments.Text.Domain.Algorithms;
using UniDocuments.Text.Domain.Services;

namespace UniDocuments.Text.Application.Similarity;

public interface IDocumentSimilarityService : IDocumentService
{
    Task<UniDocumentsCompareResult> CompareAsync(
        UniDocumentsCompareRequest request, CancellationToken cancellationToken);
}