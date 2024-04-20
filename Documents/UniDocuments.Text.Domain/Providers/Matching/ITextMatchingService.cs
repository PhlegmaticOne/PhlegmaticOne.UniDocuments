using UniDocuments.Text.Domain.Providers.Matching.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Responses;

namespace UniDocuments.Text.Domain.Providers.Matching;

public interface ITextMatchingService
{
    Task<MatchTextsResponse> MatchAsync(MatchTextsRequest request, CancellationToken cancellationToken);
}