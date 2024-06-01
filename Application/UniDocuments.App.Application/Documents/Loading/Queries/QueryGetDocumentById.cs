using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.App.Application.Documents.Loading.Queries;

public class QueryGetDocumentById : IOperationResultQuery<IDocumentLoadResponse>
{
    public QueryGetDocumentById(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }
}

public class QueryGetDocumentByIdHandler : IOperationResultQueryHandler<QueryGetDocumentById, IDocumentLoadResponse>
{
    private const string ErrorMessage = "GetDocumentById.InternalError";
    private const string ErrorMessageNotFound = "GetDocumentById.NotFound";
    
    private readonly IDocumentsStorage _documentsStorage;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<QueryGetDocumentByIdHandler> _logger;

    public QueryGetDocumentByIdHandler(
        IDocumentsStorage documentsStorage, 
        ApplicationDbContext dbContext, 
        ILogger<QueryGetDocumentByIdHandler> logger)
    {
        _documentsStorage = documentsStorage;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<OperationResult<IDocumentLoadResponse>> Handle(
        QueryGetDocumentById request, CancellationToken cancellationToken)
    {
        try
        {
            var fileName = await _dbContext.Set<StudyDocument>()
                .Where(x => x.Id == request.Id)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrEmpty(fileName))
            {
                return OperationResult.Failed<IDocumentLoadResponse>(ErrorMessageNotFound);
            }
            
            var response = await _documentsStorage.LoadAsync(request.Id, cancellationToken);
            response!.Name = fileName;
            return OperationResult.Successful(response);
        }
        catch(Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<IDocumentLoadResponse>(ErrorMessage, e.Message);
        }
    }
}