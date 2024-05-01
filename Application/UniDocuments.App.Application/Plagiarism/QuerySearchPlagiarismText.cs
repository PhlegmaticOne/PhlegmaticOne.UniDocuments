using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismText : IOperationResultQuery<PlagiarismSearchResponseText>
{
    public string Text { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public int TopN { get; set; }
}

public class QuerySearchPlagiarismTextHandler : 
    IOperationResultQueryHandler<QuerySearchPlagiarismText, PlagiarismSearchResponseText>
{
    private readonly IPlagiarismSearchProvider _plagiarismSearchProvider;

    public QuerySearchPlagiarismTextHandler(IPlagiarismSearchProvider plagiarismSearchProvider)
    {
        _plagiarismSearchProvider = plagiarismSearchProvider;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponseText>> Handle(
        QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        try
        {
            var document = UniDocument.FromString(request.Text);
            var algorithmData = new PlagiarismSearchAlgorithmData(false, request.ModelName);
            var searchRequest = new PlagiarismSearchRequest(document, request.TopN, algorithmData);
            var topParagraphs = await _plagiarismSearchProvider.SearchAsync(searchRequest, cancellationToken);
            var result = new PlagiarismSearchResponseText(topParagraphs.SuspiciousParagraphs);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult
                .Failed<PlagiarismSearchResponseText>("SearchPlagiarismText.InternalError", e.Message);
        }
    }
}