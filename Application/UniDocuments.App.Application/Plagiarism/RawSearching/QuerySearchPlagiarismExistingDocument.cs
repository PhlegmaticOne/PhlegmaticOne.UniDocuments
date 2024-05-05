using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismExistingDocument : IOperationResultQuery<PlagiarismSearchResponseDocument>
{
    public Guid DocumentId { get; set; }
    public int TopN { get; set; }
    public string ModelName { get; set; } = null!;
}

public class QuerySearchPlagiarismDocumentExistingHandler : IOperationResultQueryHandler<QuerySearchPlagiarismExistingDocument, PlagiarismSearchResponseDocument>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public QuerySearchPlagiarismDocumentExistingHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IDocumentLoadingProvider loadingProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismExistingDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, request.ModelName); 
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