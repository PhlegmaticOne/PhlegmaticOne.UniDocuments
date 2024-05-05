using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Domain.Providers.Comparing;

public interface ITextCompareProvider
{
    CompareTextsResponse Compare(CompareTextsRequest request);
    Task<CompareTextsResponse> CompareAsync(CompareTextsRequest request, CancellationToken cancellationToken);
}