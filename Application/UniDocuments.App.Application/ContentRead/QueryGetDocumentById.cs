using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;

namespace UniDocuments.App.Application.ContentRead;

public class QueryGetDocumentById : IOperationResultQuery<Stream>
{
    public Guid Id { get; }

    public QueryGetDocumentById(Guid id)
    {
        Id = id;
    }
}

public class QueryGetDocumentByIdHandler : IOperationResultQueryHandler<QueryGetDocumentById, Stream>
{
    private readonly IDocumentsStorage _documentsStorage;

    public QueryGetDocumentByIdHandler(IDocumentsStorage documentsStorage)
    {
        _documentsStorage = documentsStorage;
    }
    
    public async Task<OperationResult<Stream>> Handle(
        QueryGetDocumentById request, CancellationToken cancellationToken)
    {
        var loadRequest = new DocumentLoadRequest(request.Id);

        try
        {
            var response = await _documentsStorage.LoadAsync(loadRequest, cancellationToken);
            return OperationResult.Successful(response.Stream!);
        }
        catch
        {
            return OperationResult.Failed<Stream>("Error.InternalLoadDocumentError");
        }
    }
}