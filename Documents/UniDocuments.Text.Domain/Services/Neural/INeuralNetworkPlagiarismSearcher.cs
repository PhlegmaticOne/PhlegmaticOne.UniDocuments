using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.Text.Domain.Services.Neural;

public interface INeuralNetworkPlagiarismSearcher
{
    Task<List<ParagraphPlagiarismData>> SearchAsync(PlagiarismSearchRequest request, CancellationToken cancellationToken);
}