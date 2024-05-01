using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Plagiarism;

public class QueryPreprocessText : IOperationResultQuery<string>
{
    public string Text { get; set; } = null!;
}

public class QueryPreprocessTextHandler : IOperationResultQueryHandler<QueryPreprocessText, string>
{
    private readonly IDocumentTextPreprocessor _textPreprocessor;

    public QueryPreprocessTextHandler(IDocumentTextPreprocessor textPreprocessor)
    {
        _textPreprocessor = textPreprocessor;
    }
    
    public async Task<OperationResult<string>> Handle(QueryPreprocessText request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _textPreprocessor.Preprocess(request.Text);
            return OperationResult.Successful(result);
        }
        catch (Exception e)
        {
            return OperationResult.Failed<string>("TextPreprocess.InternalError", e.Message);
        }
    }
}