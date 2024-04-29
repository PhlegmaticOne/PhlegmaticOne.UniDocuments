using MediatR;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Plagiarism;

public class QuerySearchPlagiarismText : IOperationResultQuery<PlagiarismSearchResponse>
{
    public string Text { get; }
    public int TopN { get; }

    public QuerySearchPlagiarismText(string text, int topN)
    {
        Text = text;
        TopN = topN;
    }
}

public class QuerySearchPlagiarismTextHandler : IOperationResultQueryHandler<QuerySearchPlagiarismText, PlagiarismSearchResponse>
{
    private readonly IDocumentsNeuralModel _documentsNeuralModel;

    public QuerySearchPlagiarismTextHandler(IDocumentsNeuralModel documentsNeuralModel)
    {
        _documentsNeuralModel = documentsNeuralModel;
    }
    
    public async Task<OperationResult<PlagiarismSearchResponse>> Handle(
        QuerySearchPlagiarismText request, CancellationToken cancellationToken)
    {
        var document = new UniDocument(Guid.Empty, UniDocumentContent.FromString(request.Text));
        
        var topParagraphs = await _documentsNeuralModel
            .FindSimilarAsync(document, request.TopN, cancellationToken);
        
        var result = new PlagiarismSearchResponse(topParagraphs, Array.Empty<DocumentSearchData>());
        return OperationResult.Successful(result);
    }
}