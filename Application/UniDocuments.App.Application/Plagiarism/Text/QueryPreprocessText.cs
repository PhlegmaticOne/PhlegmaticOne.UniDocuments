using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.App.Application.Plagiarism.Text;

public class QueryPreprocessText : IOperationResultQuery<string>
{
    public string Text { get; set; } = null!;
}

public class QueryPreprocessTextHandler : IOperationResultQueryHandler<QueryPreprocessText, string>
{
    private const string TextPreprocessInternalError = "TextPreprocess.InternalError";
    
    private readonly IDocumentTextPreprocessor _textPreprocessor;
    private readonly ILogger<QueryPreprocessTextHandler> _logger;

    public QueryPreprocessTextHandler(
        IDocumentTextPreprocessor textPreprocessor,
        ILogger<QueryPreprocessTextHandler> logger)
    {
        _textPreprocessor = textPreprocessor;
        _logger = logger;
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
            _logger.LogCritical(e, TextPreprocessInternalError);
            return OperationResult.Failed<string>(TextPreprocessInternalError, e.Message);
        }
    }
}