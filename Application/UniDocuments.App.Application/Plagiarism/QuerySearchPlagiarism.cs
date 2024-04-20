using MediatR;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarism : IRequest<PlagiarismSearchResponse>
{
    public Guid DocumentId { get; }
    public int TopN { get; }

    public QuerySearchPlagiarism(Guid documentId, int topN)
    {
        DocumentId = documentId;
        TopN = topN;
    }
}

public class QuerySearchPlagiarismHandler : IRequestHandler<QuerySearchPlagiarism, PlagiarismSearchResponse>
{
    private readonly IPlagiarismSearcher _plagiarismSearcher;

    public QuerySearchPlagiarismHandler(IPlagiarismSearcher plagiarismSearcher)
    {
        _plagiarismSearcher = plagiarismSearcher;
    }
    
    public Task<PlagiarismSearchResponse> Handle(QuerySearchPlagiarism request, CancellationToken cancellationToken)
    {
        var searchRequest = new PlagiarismSearchRequest(request.DocumentId, request.TopN, ""); 
        return _plagiarismSearcher.SearchAsync(searchRequest, cancellationToken);
    }
}