using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Application.Plagiarism.RawSearching.Base;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism.RawSearching;

public class QuerySearchPlagiarismText : QuerySearchPlagiarism
{
    public string Text { get; set; } = null!;
}

public class QuerySearchPlagiarismTextHandler : 
    IOperationResultQueryHandler<QuerySearchPlagiarismText, PlagiarismSearchResponseDocument>
{
    private const string SearchPlagiarismTextInternalError = "SearchPlagiarismText.InternalError";
    
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;
    private readonly ILogger<QuerySearchPlagiarismTextHandler> _logger;

    public QuerySearchPlagiarismTextHandler(
        IPlagiarismSearchProvider plagiarismSearchProvider,
        ILogger<QuerySearchPlagiarismTextHandler> logger)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponseDocument>> Handle(
        QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopCount, request.ModelName);
            var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, SearchPlagiarismTextInternalError);
            return OperationResult.Failed<PlagiarismSearchResponseDocument>(SearchPlagiarismTextInternalError, e.Message);
        }
    }
}