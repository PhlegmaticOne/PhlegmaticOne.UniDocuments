using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Matching;
using UniDocuments.Text.Domain.Providers.Matching.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Responses;

namespace UniDocuments.App.Application.Documents.Plagiarism.Text;

public class QueryMatchTexts : IOperationResultQuery<MatchTextsResponse>
{
    public MatchTextsRequest Request { get; }

    public QueryMatchTexts(MatchTextsRequest request)
    {
        Request = request;
    }
}

public class QueryMatchTextsRequestHandler : IOperationResultQueryHandler<QueryMatchTexts, MatchTextsResponse>
{
    private const string MatchTextInternalError = "MatchText.InternalError";
    
    private readonly ITextMatchProvider _textMatchProvider;
    private readonly ILogger<QueryMatchTextsRequestHandler> _logger;

    public QueryMatchTextsRequestHandler(
        ITextMatchProvider textMatchProvider,
        ILogger<QueryMatchTextsRequestHandler> logger)
    {
        _textMatchProvider = textMatchProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<MatchTextsResponse>> Handle(
        QueryMatchTexts request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _textMatchProvider.MatchAsync(request.Request, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, MatchTextInternalError);
            return OperationResult.Failed<MatchTextsResponse>(MatchTextInternalError, e.Message);
        }
    }
}