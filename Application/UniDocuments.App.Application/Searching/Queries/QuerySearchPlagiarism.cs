using MediatR;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Searching.Queries;

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
    private readonly IPlagiarismFinder _plagiarismFinder;

    public QuerySearchPlagiarismHandler(IPlagiarismFinder plagiarismFinder)
    {
        _plagiarismFinder = plagiarismFinder;
    }
    
    public Task<PlagiarismSearchResponse> Handle(QuerySearchPlagiarism request, CancellationToken cancellationToken)
    {
        var searchRequest = new PlagiarismSearchRequest(request.DocumentId, request.TopN); 
        return _plagiarismFinder.SearchAsync(searchRequest, cancellationToken);
    }
}