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
    private readonly ITextMatchProvider _textMatchProvider;

    public QueryMatchTextsRequestHandler(ITextMatchProvider textMatchProvider)
    {
        _textMatchProvider = textMatchProvider;
    }
    
    public Task<MatchTextsResponse> Handle(QueryMatchTexts request, CancellationToken cancellationToken)
    {
        return _textMatchProvider.MatchAsync(request.Request, cancellationToken);
    }
}