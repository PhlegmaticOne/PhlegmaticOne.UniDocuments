using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.Text.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralSourceInMemory : IDocumentsNeuralSource
{
    private readonly IFileStorageIndexable _fileStorageIndexable;
    private readonly IStreamContentReader _streamContentReader;

    private int _currentIndex;

    public DocumentNeuralSourceInMemory(IFileStorageIndexable fileStorageIndexable, IStreamContentReader streamContentReader)
    {
        _fileStorageIndexable = fileStorageIndexable;
        _streamContentReader = streamContentReader;
    }
    
    public Task InitializeAsync()
    {
        _currentIndex = -1;
        return Task.CompletedTask;
    }

    public async Task<RawDocument> GetNextDocumentAsync()
    {
        if (_currentIndex >= _fileStorageIndexable.StorageSize - 1)
        {
            return RawDocument.NoData();
        }

        _currentIndex += 1;
        var result = _fileStorageIndexable.Load(_currentIndex);
        var readResult = await _streamContentReader.ReadAsync(result.FileStream!, CancellationToken.None);
        return new RawDocument(result.FileId, readResult.Paragraphs);
    }

    public void Dispose()
    {
        _currentIndex = -1;
    }
}