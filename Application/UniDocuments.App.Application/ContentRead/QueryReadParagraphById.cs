using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.ContentReading;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadParagraphById : IOperationResultQuery<string>
{
    public int ParagraphId { get; set; }
}

public class QueryReadParagraphByIdHandler : IOperationResultQueryHandler<QueryReadParagraphById, string>
{
    private const string ErrorMessage = "ReadParagraphById.InternalError";
    private const string ParagraphNotFoundMessage = "ReadParagraphById.ParagraphNotExist";
    
    private readonly IParagraphGlobalReader _paragraphGlobalReader;
    private readonly ILogger<QueryReadParagraphByIdHandler> _logger;

    public QueryReadParagraphByIdHandler(
        IParagraphGlobalReader paragraphGlobalReader,
        ILogger<QueryReadParagraphByIdHandler> logger)
    {
        _paragraphGlobalReader = paragraphGlobalReader;
        _logger = logger;
    }
    
    public async Task<OperationResult<string>> Handle(
        QueryReadParagraphById request, CancellationToken cancellationToken)
    {
        try
        {
            var paragraph = await _paragraphGlobalReader.ReadAsync(request.ParagraphId, cancellationToken);

            return string.IsNullOrEmpty(paragraph) ?
                OperationResult.Failed<string>(ParagraphNotFoundMessage) :
                OperationResult.Successful(paragraph);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<string>(ErrorMessage, e.Message);
        }
    }
}