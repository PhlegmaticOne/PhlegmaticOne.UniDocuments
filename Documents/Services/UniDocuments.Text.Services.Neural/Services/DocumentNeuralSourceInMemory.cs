using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Domain.Shared;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralSourceInMemory : IDocumentsNeuralSource
{
    private readonly IDocumentStorageIndexable _documentStorageIndexable;
    private readonly IStreamContentReader _streamContentReader;

    private int _currentIndex;

    public DocumentNeuralSourceInMemory(IDocumentStorageIndexable documentStorageIndexable, IStreamContentReader streamContentReader)
    {
        _documentStorageIndexable = documentStorageIndexable;
        _streamContentReader = streamContentReader;
    }
    
    public Task InitializeAsync()
    {
        _currentIndex = -1;
        return Task.CompletedTask;
    }

    public async Task<RawDocument> GetNextDocumentAsync()
    {
        if (_currentIndex >= _documentStorageIndexable.StorageSize - 1)
        {
            return RawDocument.NoData();
        }

        _currentIndex += 1;
        var result = _documentStorageIndexable.Load(_currentIndex);
        var readResult = await _streamContentReader.ReadAsync(result.Stream!, CancellationToken.None);
        return new RawDocument(result.Id, readResult.Paragraphs);
    }

    public void Dispose()
    {
        _currentIndex = -1;
    }
}