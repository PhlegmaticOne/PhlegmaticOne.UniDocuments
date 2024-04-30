using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Services.FileStorage.InMemory;

public class DocumentsStorageInMemory : IDocumentsStorage
{
    private class FileLoadResponsePrepared
    {
        private readonly Guid _fileId;
        private readonly string _fileName;
        private readonly byte[] _fileContent;

        public FileLoadResponsePrepared(Guid fileId, string fileName, byte[] fileContent)
        {
            _fileId = fileId;
            _fileName = fileName;
            _fileContent = fileContent;
        }

        public DocumentLoadResponse ToFileLoadResponse()
        {
            var stream = new MemoryStream(_fileContent);
            return new DocumentLoadResponse(_fileId, _fileName, stream);
        }
    }
    
    private readonly Dictionary<Guid, FileLoadResponsePrepared> _fileContents = new();

    public int StorageSize => _fileContents.Count;

    public Task<DocumentLoadResponse> LoadAsync(DocumentLoadRequest loadRequest, CancellationToken cancellationToken)
    {
        var fileId = loadRequest.Id;
        
        var result = _fileContents.TryGetValue(fileId, out var fileLoadResponsePrepared) ? 
            fileLoadResponsePrepared.ToFileLoadResponse() : 
            DocumentLoadResponse.NoContent();

        return Task.FromResult(result);
    }

    public async Task SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var fileBytes = await GetFileContentAsync(saveRequest, cancellationToken);
        var fileId = saveRequest.Id;
        _fileContents[fileId] = new FileLoadResponsePrepared(fileId, saveRequest.Name, fileBytes);
    }

    private static async Task<byte[]> GetFileContentAsync(
        DocumentSaveRequest documentSaveRequest, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await documentSaveRequest.Stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}