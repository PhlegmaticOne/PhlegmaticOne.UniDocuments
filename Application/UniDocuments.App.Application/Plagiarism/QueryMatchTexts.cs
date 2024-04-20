using MediatR;
using UniDocuments.Text.Domain.Providers.Matching;
using UniDocuments.Text.Domain.Providers.Matching.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QueryMatchTexts : IRequest<MatchTextsResponse>
{
    public MatchTextsRequest Request { get; }

    public QueryMatchTexts(MatchTextsRequest request)
    {
        Request = request;
    }
}

public class QueryMatchTextsRequestHandler : IRequestHandler<QueryMatchTexts, MatchTextsResponse>
{
    private readonly ITextMatchingService _textMatchingService;

    public QueryMatchTextsRequestHandler(ITextMatchingService textMatchingService)
    {
        _textMatchingService = textMatchingService;
    }
    
    public Task<MatchTextsResponse> Handle(QueryMatchTexts request, CancellationToken cancellationToken)
    {
        return _textMatchingService.MatchAsync(request.Request, cancellationToken);
    }
}