using MediatR;
using UniDocuments.Text.Domain.Providers.Similarity;
using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Providers.Similarity.Responses;

namespace UniDocuments.App.Application.Comparing.Queries;

public class QueryCompareStringsByAlgorithms : IRequest<List<SimilarityResponse>>
{
    public TextsSimilarityRequest Request { get; }

    public QueryCompareStringsByAlgorithms(TextsSimilarityRequest request)
    {
        Request = request;
    }
}

public class QueryCompareStringsByAlgorithmsRequestHandler :
    IRequestHandler<QueryCompareStringsByAlgorithms, List<SimilarityResponse>>
{
    private readonly IDocumentsSimilarityFinder _similarityFinder;

    public QueryCompareStringsByAlgorithmsRequestHandler(IDocumentsSimilarityFinder similarityFinder)
    {
        _similarityFinder = similarityFinder;
    }
    
    public Task<List<SimilarityResponse>> Handle(QueryCompareStringsByAlgorithms request, CancellationToken cancellationToken)
    {
        return _similarityFinder.CompareAsync(request.Request, cancellationToken);
    }
}