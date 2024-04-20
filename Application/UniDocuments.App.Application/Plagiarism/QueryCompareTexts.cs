using MediatR;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QueryCompareTexts : IRequest<CompareTextsResponse>
{
    public CompareTextsRequest Request { get; }

    public QueryCompareTexts(CompareTextsRequest request)
    {
        Request = request;
    }
}

public class QueryCompareTextsRequestHandler : IRequestHandler<QueryCompareTexts, CompareTextsResponse>
{
    private readonly ICompareTextsService _similarityService;

    public QueryCompareTextsRequestHandler(ICompareTextsService similarityService)
    {
        _similarityService = similarityService;
    }
    
    public Task<CompareTextsResponse> Handle(QueryCompareTexts request, CancellationToken cancellationToken)
    {
        return _similarityService.CompareAsync(request.Request, cancellationToken);
    }
}