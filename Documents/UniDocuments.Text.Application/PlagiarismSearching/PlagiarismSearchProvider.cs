using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly INeuralNetworkPlagiarismSearcher _networkPlagiarismSearcher;
    private readonly IDocumentMapper _documentMapper;

    public PlagiarismSearchProvider(
        INeuralNetworkPlagiarismSearcher networkPlagiarismSearcher,
        IDocumentMapper documentMapper)
    {
        _networkPlagiarismSearcher = networkPlagiarismSearcher;
        _documentMapper = documentMapper;
    }
    
    public async Task<PlagiarismSearchResponseDocument> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var id = request.Document.Id;
        var documentData = _documentMapper.GetDocumentData(id);
        var response = new PlagiarismSearchResponseDocument(id, documentData?.Name ?? string.Empty);

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