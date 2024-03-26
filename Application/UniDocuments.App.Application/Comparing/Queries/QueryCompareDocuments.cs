using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Application.Similarity;
using UniDocuments.Text.Domain.Algorithms;

namespace UniDocuments.App.Application.Comparing.Queries;

public class QueryCompareDocuments : IdentityOperationResultQuery<UniDocumentsCompareResult>
{
    public Guid ComparingDocumentId { get; }
    public Guid OriginalDocumentId { get; }
    public List<string> Algorithms { get; }

    public QueryCompareDocuments(Guid profileId, Guid comparingDocumentId,
        Guid originalDocumentId, List<string> algorithms) : base(profileId)
    {
        ComparingDocumentId = comparingDocumentId;
        OriginalDocumentId = originalDocumentId;
        Algorithms = algorithms;
    }
}

public class CommandCompareDocumentsHandler :
    IOperationResultQueryHandler<QueryCompareDocuments, UniDocumentsCompareResult>
{
    private readonly IDocumentSimilarityService _similarityService;

    public CommandCompareDocumentsHandler(IDocumentSimilarityService similarityService)
    {
        _similarityService = similarityService;
    }
    
    public async Task<OperationResult<UniDocumentsCompareResult>> Handle(
        QueryCompareDocuments request, CancellationToken cancellationToken)
    {
        try
        {
            var compareRequest = new UniDocumentsCompareRequest(
                request.ComparingDocumentId, request.OriginalDocumentId, request.Algorithms);
            
            var result = await _similarityService.CompareAsync(compareRequest, cancellationToken);
            
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed<UniDocumentsCompareResult>(e.Message);
        }
    }
}