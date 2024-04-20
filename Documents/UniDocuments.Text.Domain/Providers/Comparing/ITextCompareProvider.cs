using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Domain.Providers.Comparing;

public interface ITextCompareProvider
{
    Task<CompareTextsResponse> CompareAsync(CompareTextsRequest request, CancellationToken cancellationToken);
}