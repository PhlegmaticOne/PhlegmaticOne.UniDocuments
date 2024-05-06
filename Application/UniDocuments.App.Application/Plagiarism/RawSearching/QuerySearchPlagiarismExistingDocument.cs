using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.RawSearching.Base;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismExistingDocument : QuerySearchPlagiarism
{
    public Guid DocumentId { get; set; }
}

public class QuerySearchPlagiarismDocumentExistingHandler : IOperationResultQueryHandler<QuerySearchPlagiarismExistingDocument, PlagiarismSearchResponseDocument>
{
    private const string? SearchPlagiarismDocumentInternalError = "SearchPlagiarismDocument.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly IDocumentLoadingProvider _loadingProvider;
    private readonly ILogger<QuerySearchPlagiarismDocumentExistingHandler> _logger;

    public QuerySearchPlagiarismDocumentExistingHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        IDocumentLoadingProvider loadingProvider,
        ILogger<QuerySearchPlagiarismDocumentExistingHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _loadingProvider = loadingProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismExistingDocument request, CancellationToken cancellationToken)
    {
        try
        {
            var document = await _loadingProvider.LoadAsync(request.DocumentId, true, cancellationToken);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopCount, request.InferEpochs, request.ModelName); 
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, SearchPlagiarismDocumentInternalError);
            return OperationResult
                .Failed<PlagiarismSearchResponseDocument>(SearchPlagiarismDocumentInternalError, e.Message);
        }
    }
}