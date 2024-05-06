using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadDocumentContentById : IOperationResultQuery<UniDocument>
{
    public Guid Id { get; set; }
}

public class QueryReadDocumentContentByIdHandler : 
    IOperationResultQueryHandler<QueryReadDocumentContentById, UniDocument>
{
    private const string ErrorMessage = "ReadDocumentById.InternalError";
    private const string DocumentNotFoundMessage = "ReadDocumentById.DocumentNotExist";
    
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;
    private readonly ILogger<QueryReadDocumentContentByIdHandler> _logger;

    public QueryReadDocumentContentByIdHandler(
        IDocumentMapper documentMapper,
        IDocumentLoadingProvider loadingProvider,
        ILogger<QueryReadDocumentContentByIdHandler> logger)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<UniDocument>> Handle(
        QueryReadDocumentContentById request, CancellationToken cancellationToken)
    {
        try
        {
            var documentData = _documentMapper.GetDocumentData(request.Id);

            if (documentData is null)
            {
                return OperationResult.Failed<UniDocument>(DocumentNotFoundMessage);
            }

            var document = await _loadingProvider.LoadAsync(documentData.Id, true, cancellationToken);
            return OperationResult.Successful(document);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<UniDocument>(ErrorMessage, e.Message);
        }
    }
}