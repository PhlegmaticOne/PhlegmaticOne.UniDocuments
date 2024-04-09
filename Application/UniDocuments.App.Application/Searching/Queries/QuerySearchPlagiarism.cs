using MediatR;
using UniDocuments.Text.Domain.Services.Searching;
using UniDocuments.Text.Domain.Services.Searching.Request;
using UniDocuments.Text.Domain.Services.Searching.Response;

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
    private readonly IPlagiarismSearcher _plagiarismSearcher;

    public QuerySearchPlagiarismHandler(IPlagiarismSearcher plagiarismSearcher)
    {
        _plagiarismSearcher = plagiarismSearcher;
    }
    
    public Task<PlagiarismSearchResponse> Handle(QuerySearchPlagiarism request, CancellationToken cancellationToken)
    {
        return _plagiarismSearcher.SearchAsync(new PlagiarismSearchRequest(request.DocumentId, request.TopN));
    }
}