using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismDocument : IOperationResultQuery<PlagiarismSearchResponseDocument>
{
    public Guid DocumentId { get; set; }
    public int TopN { get; set; }
    public bool UseFingerprint { get; set; }
    public string ModelName { get; set; } = null!;
}

public class QuerySearchPlagiarismDocumentHandler : IOperationResultQueryHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponseDocument>
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
    
    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var algorithmData = new PlagiarismSearchAlgorithmData(request.UseFingerprint, request.ModelName);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, algorithmData); 
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult
                .Failed<PlagiarismSearchResponseDocument>("SearchPlagiarismDocument.InternalError", e.Message);
        }
    }
}