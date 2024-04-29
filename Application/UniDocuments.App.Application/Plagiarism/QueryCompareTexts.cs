using MediatR;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.App.Application.Plagiarism;

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
    private readonly ITextCompareProvider _similarityProvider;

    public QueryCompareTextsRequestHandler(ITextCompareProvider similarityProvider)
    {
        _similarityProvider = similarityProvider;
    }
    
    public async Task<OperationResult<CompareTextsResponse>> Handle(
        QueryCompareTexts request, CancellationToken cancellationToken)
    {
        var result = await _similarityProvider.CompareAsync(request.Request, cancellationToken);
        return OperationResult.Successful(result);
    }
}