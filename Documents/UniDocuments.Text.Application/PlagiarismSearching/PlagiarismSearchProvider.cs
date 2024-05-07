using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly INeuralNetworkPlagiarismSearcher _networkPlagiarismSearcher;

    public PlagiarismSearchProvider(INeuralNetworkPlagiarismSearcher networkPlagiarismSearcher)
    {
        _networkPlagiarismSearcher = networkPlagiarismSearcher;
    }
    
    public async Task<PlagiarismSearchResponseDocument> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var response = new PlagiarismSearchResponseDocument(request.Document.Id, request.Document.Name);
        await FindNeuralPlagiarismAsync(request, response, cancellationToken);
        return response;
    }

    private async Task FindNeuralPlagiarismAsync(
        PlagiarismSearchRequest request, PlagiarismSearchResponseDocument response, CancellationToken cancellationToken)
    {
        var result = await _networkPlagiarismSearcher.SearchAsync(request, cancellationToken);
        response.SuspiciousParagraphs = result;
    }
}