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
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;

    public QuerySearchPlagiarismHandler(IPlagiarismSearchProvider plagiarismSearchProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
    }
    
    public Task<PlagiarismSearchResponse> Handle(QuerySearchPlagiarism request, CancellationToken cancellationToken)
    {
        var searchRequest = new PlagiarismSearchRequest(request.DocumentId, request.TopN, ""); 
        return _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
    }
}