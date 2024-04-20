using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;

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

    public async Task<UniDocument> GetNextDocumentAsync()
    {
        if (_currentIndex >= _documentStorageIndexable.StorageSize - 1)
        {
            return UniDocument.Empty;
        }

        _currentIndex += 1;
        var result = _documentStorageIndexable.Load(_currentIndex);
        var readResult = await _streamContentReader.ReadAsync(result.Stream!, CancellationToken.None);
        return new UniDocument(result.Id, readResult);
    }

    public void Dispose()
    {
        _currentIndex = -1;
    }
}