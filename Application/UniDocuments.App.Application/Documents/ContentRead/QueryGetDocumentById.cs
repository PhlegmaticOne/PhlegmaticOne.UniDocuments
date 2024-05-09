using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.App.Application.Documents.ContentRead;

public class QueryGetDocumentById : IOperationResultQuery<DocumentLoadResponse>
{
    public Guid Id { get; set; }
}

public class QueryGetDocumentByIdHandler : IOperationResultQueryHandler<QueryGetDocumentById, DocumentLoadResponse>
{
    private const string ErrorMessage = "GetDocumentById.InternalError";
    
    private readonly IDocumentsStorage _documentsStorage;
    private readonly ILogger<QueryGetDocumentByIdHandler> _logger;

    public QueryGetDocumentByIdHandler(IDocumentsStorage documentsStorage, ILogger<QueryGetDocumentByIdHandler> logger)
    {
        _documentsStorage = documentsStorage;
        _logger = logger;
    }
    
    public async Task<OperationResult<DocumentLoadResponse>> Handle(
        QueryGetDocumentById request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _documentsStorage.LoadAsync(request.Id, cancellationToken);
            return OperationResult.Successful(response);
        }
        catch(Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<DocumentLoadResponse>(ErrorMessage, e.Message);
        }
    }
}