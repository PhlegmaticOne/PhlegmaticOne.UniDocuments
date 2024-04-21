using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentMapping.Models;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.Neural.Sources;

public class DocumentNeuralSourceInMemory : IDocumentsNeuralSource
{
    private readonly IDocumentsStorage _documentsStorage;
    private readonly IDocumentMapper _documentMapper;
    private readonly IStreamContentReader _streamContentReader;

    private CancellationToken _cancellationToken;
    private int _currentIndex;

    public DocumentNeuralSourceInMemory(
        IDocumentsStorage documentsStorage, 
        IDocumentMapper documentMapper,
        IStreamContentReader streamContentReader)
    {
        _documentsStorage = documentsStorage;
        _documentMapper = documentMapper;
        _streamContentReader = streamContentReader;
    }
    
    public Task InitializeAsync()
    {
        _currentIndex = 0;
        _cancellationToken = CancellationToken.None;
        return Task.CompletedTask;
    }

    public async Task<DocumentNeuralViewModel> GetNextDocumentAsync()
    {
        var documentData = _documentMapper.GetDocumentData(_currentIndex);

        if (documentData is null)
        {
            return DocumentNeuralViewModel.Empty;
        }

        _currentIndex += 1;
        
        var result = await _documentsStorage.LoadAsync(new DocumentLoadRequest(documentData.Id), _cancellationToken);
        var content = await _streamContentReader.ReadAsync(result.Stream!, _cancellationToken);
        var resultParagraphs = CreateParagraphs(content, documentData);
        
        return new DocumentNeuralViewModel(0, resultParagraphs);
    }

    public void Dispose()
    {
        _currentIndex = 0;
    }

    private static List<ParagraphNeuralViewModel> CreateParagraphs(
        UniDocumentContent content, DocumentGlobalMapData documentData)
    {
        var resultParagraphs = new List<ParagraphNeuralViewModel>();

        for (var i = 0; i < content.Paragraphs.Count; i++)
        {
            var paragraph = content.Paragraphs[i];
            var globalId = documentData.GlobalFirstParagraphId + i;
            resultParagraphs.Add(new ParagraphNeuralViewModel(globalId, paragraph));
        }

        return resultParagraphs;
    }
}