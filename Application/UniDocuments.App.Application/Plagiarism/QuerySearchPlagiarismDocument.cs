using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismDocument : IOperationResultQuery<PlagiarismSearchResponse>
{
    public Guid DocumentId { get; }
    public int TopN { get; }

    public QuerySearchPlagiarismDocument(Guid documentId, int topN)
    {
        DocumentId = documentId;
        TopN = topN;
    }
}

public class QuerySearchPlagiarismDocumentHandler : IOperationResultQueryHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponse>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;

    public QuerySearchPlagiarismDocumentHandler(IPlagiarismSearchProvider plagiarismSearchProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponse>> Handle(
        QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        var searchRequest = new PlagiarismSearchRequest(request.DocumentId, request.TopN, ""); 
        var result = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
        return OperationResult.Successful(result);
    }
}