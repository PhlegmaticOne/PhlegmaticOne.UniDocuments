using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadDocumentContentById : IOperationResultQuery<UniDocument>
{
    public Guid Id { get; }

    public QueryReadDocumentContentById(Guid id)
    {
        Id = id;
    }
}

public class QueryReadDocumentContentByIdHandler : 
    IOperationResultQueryHandler<QueryReadDocumentContentById, UniDocument>
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public QueryReadDocumentContentByIdHandler(IDocumentMapper documentMapper, IDocumentLoadingProvider loadingProvider)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<OperationResult<UniDocument>> Handle(
        QueryReadDocumentContentById request, CancellationToken cancellationToken)
    {
        var documentData = _documentMapper.GetDocumentData(request.Id);

        if (documentData is null)
        {
            return OperationResult.Failed<UniDocument>("Error.NoDocumentWithId");
        }

        var document = await _loadingProvider.LoadAsync(documentData.Id, true, cancellationToken);
        return OperationResult.Successful(document);
    }
}