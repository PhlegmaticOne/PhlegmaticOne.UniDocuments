using MediatR;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismDocument : IRequest<PlagiarismSearchResponse>
{
    public Guid DocumentId { get; }
    public int TopN { get; }

    public QuerySearchPlagiarismDocument(Guid documentId, int topN)
    {
        DocumentId = documentId;
        TopN = topN;
    }
}

public class QuerySearchPlagiarismDocumentHandler : IRequestHandler<QuerySearchPlagiarismDocument, PlagiarismSearchResponse>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;

    public QuerySearchPlagiarismDocumentHandler(IPlagiarismSearchProvider plagiarismSearchProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
    }
    
    public Task<PlagiarismSearchResponse> Handle(QuerySearchPlagiarismDocument request, CancellationToken cancellationToken)
    {
        var searchRequest = new PlagiarismSearchRequest(request.DocumentId, request.TopN, ""); 
        return _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
    }
}