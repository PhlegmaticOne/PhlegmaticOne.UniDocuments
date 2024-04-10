using UniDocuments.Text.Domain.Services.Searching.Request;
using UniDocuments.Text.Domain.Services.Searching.Response;

namespace UniDocuments.Text.Domain.Services.Searching;

public interface IPlagiarismSearcher : IDocumentService
{
    Task<PlagiarismSearchResponse> SearchAsync(PlagiarismSearchRequest request, CancellationToken cancellationToken);
}