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
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public QueryReadDocumentContentByGlobalIdHandler(IDocumentMapper documentMapper, IDocumentLoadingProvider loadingProvider)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<OperationResult<UniDocument>> Handle(
        QueryReadDocumentContentByGlobalId request, CancellationToken cancellationToken)
    {
        var documentData = _documentMapper.GetDocumentData(request.GlobalId);

        if (documentData is null)
        {
            return OperationResult.Failed<UniDocument>("ReadDocumentByGlobalId.DocumentNotExist");
        }

        var document = await _loadingProvider.LoadAsync(documentData.Id, true, cancellationToken);
        return OperationResult.Successful(document);
    }
}