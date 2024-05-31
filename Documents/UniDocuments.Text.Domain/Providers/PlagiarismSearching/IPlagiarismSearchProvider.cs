using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching;

public interface IPlagiarismSearchProvider
{
    Task<PlagiarismSearchResponse> SearchAsync(PlagiarismSearchRequest request);
}