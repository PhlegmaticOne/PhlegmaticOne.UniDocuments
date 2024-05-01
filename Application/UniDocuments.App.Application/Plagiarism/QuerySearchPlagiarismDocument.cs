using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismDocument : IOperationResultQuery<PlagiarismSearchResponse>
{
    public Guid DocumentId { get; set; }
    public int TopN { get; set; }
    public PlagiarismSearchAlgorithmData AlgorithmData { get; set; } = null!;
}

public class QuerySearchPlagiarismDocumentHandler : IOperationResultQueryHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponse>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public QuerySearchPlagiarismDocumentHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IDocumentLoadingProvider loadingProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponse>> Handle(
        QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, request.AlgorithmData); 
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult
                .Failed<PlagiarismSearchResponse>("SearchPlagiarismDocument.InternalError", e.Message);
        }
    }
}