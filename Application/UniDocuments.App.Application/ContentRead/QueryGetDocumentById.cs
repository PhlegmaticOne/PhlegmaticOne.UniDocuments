using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.App.Application.ContentRead;

public class QueryGetDocumentById : IOperationResultQuery<DocumentLoadResponse>
{
    public Guid Id { get; set; }
}

public class QueryGetDocumentByIdHandler : IOperationResultQueryHandler<QueryGetDocumentById, DocumentLoadResponse>
{
    private readonly IDocumentsStorage _documentsStorage;

    public QueryGetDocumentByIdHandler(IDocumentsStorage documentsStorage)
    {
        _documentsStorage = documentsStorage;
    }
    
    public async Task<OperationResult<DocumentLoadResponse>> Handle(
        QueryGetDocumentById request, CancellationToken cancellationToken)
    {
        try
        {
            var loadRequest = new DocumentLoadRequest(request.Id);
            var response = await _documentsStorage.LoadAsync(loadRequest, cancellationToken);
            return OperationResult.Successful(response);
        }
        catch(Exception e)
        {
            return OperationResult.Failed<DocumentLoadResponse>("GetDocumentById.InternalError", e.Message);
        }
    }
}