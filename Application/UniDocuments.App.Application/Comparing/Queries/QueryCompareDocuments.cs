using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Similarity;
using UniDocuments.Text.Domain.Services.Similarity.Request;
using UniDocuments.Text.Domain.Services.Similarity.Response;

namespace UniDocuments.App.Application.Comparing.Queries;

public class QueryCompareDocuments : IdentityOperationResultQuery<UniDocumentsCompareResponse>
{
    public UniDocumentsCompareRequest Request { get; }

    public QueryCompareDocuments(Guid profileId, UniDocumentsCompareRequest request) : base(profileId)
    {
        Request = request;
    }
}

public class CommandCompareDocumentsHandler :
    IOperationResultQueryHandler<QueryCompareDocuments, UniDocumentsCompareResponse>
{
    private readonly IDocumentSimilarityService _similarityService;

    public CommandCompareDocumentsHandler(IDocumentSimilarityService similarityService)
    {
        _similarityService = similarityService;
    }
    
    public async Task<OperationResult<UniDocumentsCompareResponse>> Handle(
        QueryCompareDocuments request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _similarityService.CompareAsync(request.Request, cancellationToken);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed<UniDocumentsCompareResponse>(e.Message);
        }
    }
}