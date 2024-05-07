using UniDocuments.Text.Domain.Providers.ContentReading;
using UniDocuments.Text.Domain.Providers.Loading;
using UniDocuments.Text.Domain.Services.DocumentMapping;

namespace UniDocuments.Text.Application.ContentReading;

public class ParagraphGlobalReader : IParagraphGlobalReader
{
    private readonly IDocumentMapper _documentMapper;
    private readonly IDocumentLoadingProvider _loadingProvider;

    public ParagraphGlobalReader(IDocumentMapper documentMapper, IDocumentLoadingProvider loadingProvider)
    {
        _documentMapper = documentMapper;
        _loadingProvider = loadingProvider;
    }
    
    public async Task<string> ReadAsync(int globalParagraphId, CancellationToken cancellationToken)
    {
        var documentId = _documentMapper.GetDocumentIdFromGlobalParagraphId(globalParagraphId);
        var documentData = _documentMapper.GetDocumentData(documentId);

        if (documentData is null)
        {
            return string.Empty;
        }

        var document = await _loadingProvider.LoadAsync(documentData.Id, true, cancellationToken);
        var localParagraphId = documentData.GetLocalParagraphId(globalParagraphId);
        var paragraph = document.Content.Paragraphs[localParagraphId];
        return paragraph;
    }
}