using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadDocumentContentByGlobalId : IOperationResultQuery<UniDocument>
{
    public int GlobalId { get; set; }
}

public class QueryReadDocumentContentByGlobalIdHandler : 
    IOperationResultQueryHandler<QueryReadDocumentContentByGlobalId, UniDocument>
{
    private const string ErrorMessage = "ReadDocumentContentByGlobalId.InternalError";
    private const string DocumentNotFoundMessage = "ReadDocumentByGlobalId.DocumentNotExist";
    
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;
    private readonly ILogger<QueryReadDocumentContentByGlobalIdHandler> _logger;

    public QueryReadDocumentContentByGlobalIdHandler(
        IDocumentMapper documentMapper,
        IDocumentLoadingProvider loadingProvider,
        ILogger<QueryReadDocumentContentByGlobalIdHandler> logger)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult<UniDocument>> Handle(
        QueryReadDocumentContentByGlobalId request, CancellationToken cancellationToken)
    {
        try
        {
            var documentData = _documentMapper.GetDocumentData(request.GlobalId);

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