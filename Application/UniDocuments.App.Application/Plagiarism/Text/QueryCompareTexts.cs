using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.App.Application.Plagiarism.Text;

public class QueryCompareTexts : IOperationResultQuery<CompareTextsResponse>
{
    public CompareTextsRequest Request { get; }

    public QueryCompareTexts(CompareTextsRequest request)
    {
        Request = request;
    }
}

public class QueryCompareTextsRequestHandler : IOperationResultQueryHandler<QueryCompareTexts, CompareTextsResponse>
{
    private const string CompareTextsInternalError = "CompareTexts.InternalError";
    
    private readonly ITextCompareProvider _similarityProvider;
    private readonly ILogger<QueryCompareTextsRequestHandler> _logger;

    public QueryCompareTextsRequestHandler(
        ITextCompareProvider similarityProvider,
        ILogger<QueryCompareTextsRequestHandler> logger)
    {
        _similarityProvider = similarityProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<CompareTextsResponse>> Handle(
        QueryCompareTexts request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _similarityProvider.CompareAsync(request.Request, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, CompareTextsInternalError);
            return OperationResult.Failed<CompareTextsResponse>(CompareTextsInternalError, e.Message);
        }
    }
}