using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.App.Application.ContentRead;

public class QueryReadParagraphById : IOperationResultQuery<string>
{
    public int ParagraphId { get; set; }
}

public class QueryReadParagraphByIdHandler : IOperationResultQueryHandler<QueryReadParagraphById, string>
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public QueryReadParagraphByIdHandler(IDocumentMapper documentMapper, IDocumentLoadingProvider loadingProvider)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<OperationResult<string>> Handle(
        QueryReadParagraphById request, CancellationToken cancellationToken)
    {
        var documentId = _documentMapper.GetDocumentIdFromGlobalParagraphId(request.ParagraphId);
        var documentData = _documentMapper.GetDocumentData(documentId);

        if (documentData is null)
        {
            return OperationResult.Failed<string>("ReadParagraphById.ParagraphNotExist");
        }

        var document = await _loadingProvider.LoadAsync(documentData.Id, true, cancellationToken);
        var localParagraphId = documentData.GetLocalParagraphId(request.ParagraphId);
        var paragraph = document.Content!.Paragraphs[localParagraphId];
        return OperationResult.Successful(paragraph);
    }
}