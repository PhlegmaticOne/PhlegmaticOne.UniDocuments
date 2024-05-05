using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Fingerprinting.Services;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Application.PlagiarismSearching;

public class PlagiarismSearchProvider : IPlagiarismSearchProvider
{
    private readonly IFingerprintPlagiarismSearcher _fingerprintPlagiarismSearcher;
    private readonly INeuralNetworkPlagiarismSearcher _networkPlagiarismSearcher;

    public PlagiarismSearchProvider(
        IFingerprintPlagiarismSearcher fingerprintPlagiarismSearcher,
        INeuralNetworkPlagiarismSearcher networkPlagiarismSearcher)
    {
        _fingerprintPlagiarismSearcher = fingerprintPlagiarismSearcher;
        _networkPlagiarismSearcher = networkPlagiarismSearcher;
    }
    
    public async Task<PlagiarismSearchResponseDocument> SearchAsync(
        PlagiarismSearchRequest request, CancellationToken cancellationToken)
    {
        var response = new PlagiarismSearchResponseDocument();
        var searchTasks = new List<Task>();

        if (request.AlgorithmData.UseFingerprint)
        {
            searchTasks.Add(FindFingerprintPlagiarismAsync(request, response, cancellationToken));
        }
        
        searchTasks.Add(FindNeuralPlagiarismAsync(request, response, cancellationToken));

        await Task.WhenAll(searchTasks);
        
        return response;
    }

    private async Task FindFingerprintPlagiarismAsync(
        PlagiarismSearchRequest request, PlagiarismSearchResponseDocument response, CancellationToken cancellationToken)
    {
        var topFingerprints = await _fingerprintPlagiarismSearcher
            .SearchTopAsync(request.Document.Id, request.NDocuments, cancellationToken);

        response.SuspiciousDocuments = topFingerprints;
    }

    private async Task FindNeuralPlagiarismAsync(
        PlagiarismSearchRequest request, PlagiarismSearchResponseDocument response, CancellationToken cancellationToken)
    {
        var result = await _networkPlagiarismSearcher.SearchAsync(request, cancellationToken);
        response.SuspiciousParagraphs = result;
    }
}