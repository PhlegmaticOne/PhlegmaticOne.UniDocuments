using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Similarity;
using UniDocuments.Text.Domain.Providers.Similarity.Requests;
using UniDocuments.Text.Domain.Providers.Similarity.Responses;

namespace UniDocuments.App.Application.Comparing.Queries;

public class QueryCompareDocuments : IdentityOperationResultQuery<SimilarityResponse>
{
    public DocumentsSimilarityRequest Request { get; }

    public QueryCompareDocuments(Guid profileId, DocumentsSimilarityRequest request) : base(profileId)
    {
        Request = request;
    }
}

public class CommandCompareDocumentsHandler :
    IOperationResultQueryHandler<QueryCompareDocuments, SimilarityResponse>
{
    private readonly IDocumentsSimilarityFinder _similarityFinder;

    public CommandCompareDocumentsHandler(IDocumentsSimilarityFinder similarityFinder)
    {
        _similarityFinder = similarityFinder;
    }
    
    public async Task<OperationResult<SimilarityResponse>> Handle(
        QueryCompareDocuments request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _similarityFinder.CompareAsync(request.Request, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed<SimilarityResponse>(e.Message);
        }
    }
}