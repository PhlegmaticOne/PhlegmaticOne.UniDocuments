using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Matching;
using UniDocuments.Text.Domain.Providers.Matching.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

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
    private readonly ITextMatchProvider _textMatchProvider;

    public QueryMatchTextsRequestHandler(ITextMatchProvider textMatchProvider)
    {
        _textMatchProvider = textMatchProvider;
    }
    
    public async Task<OperationResult<MatchTextsResponse>> Handle(
        QueryMatchTexts request, CancellationToken cancellationToken)
    {
        var result = await _textMatchProvider.MatchAsync(request.Request, cancellationToken);
        return OperationResult.Successful(result);
    }
}