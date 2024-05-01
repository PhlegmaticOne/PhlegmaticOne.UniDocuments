using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismText : IOperationResultQuery<PlagiarismSearchResponse>
{
    public string Text { get; set; }
    public int TopN { get; set; }
    public PlagiarismSearchAlgorithmData AlgorithmData { get; set; } = null!;
}

public class QuerySearchPlagiarismTextHandler : 
    IOperationResultQueryHandler<QuerySearchPlagiarismText, PlagiarismSearchResponse>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;

    public QuerySearchPlagiarismTextHandler(IPlagiarismSearchProvider plagiarismSearchProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponse>> Handle(
        QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, request.AlgorithmData);
            var topParagraphs = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            return OperationResult.Successful(topParagraphs);
        }
        catch (Exception e)
        {
            return OperationResult
                .Failed<PlagiarismSearchResponse>("SearchPlagiarismText.InternalError", e.Message);
        }
    }
}