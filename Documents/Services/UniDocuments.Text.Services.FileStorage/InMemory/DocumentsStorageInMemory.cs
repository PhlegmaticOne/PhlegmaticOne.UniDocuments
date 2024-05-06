using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;
using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Services.FileStorage.InMemory;

public class DocumentsStorageInMemory : IDocumentsStorage
{
    private class FileLoadResponsePrepared
    {
        private readonly string _fileName;
        private readonly byte[] _fileContent;

        public FileLoadResponsePrepared(string fileName, byte[] fileContent)
        {
            _fileName = fileName;
            _fileContent = fileContent;
        }

        public DocumentLoadResponse ToFileLoadResponse()
        {
            var stream = new MemoryStream(_fileContent);
            return new DocumentLoadResponse(_fileName, stream);
        }
    }
    
    private readonly Dictionary<Guid, FileLoadResponsePrepared> _fileContents = new();

    public Task<DocumentLoadResponse> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = _fileContents.TryGetValue(id, out var fileLoadResponsePrepared) ? 
            fileLoadResponsePrepared.ToFileLoadResponse() : 
            DocumentLoadResponse.NoContent();

        return Task.FromResult(result);
    }

    public async Task<Guid> SaveAsync(DocumentSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var fileBytes = await GetFileContentAsync(saveRequest, cancellationToken);
        var fileId = saveRequest.Id;
        _fileContents[fileId] = new FileLoadResponsePrepared(saveRequest.Name, fileBytes);
        return saveRequest.Id;
    }

    private static async Task<byte[]> GetFileContentAsync(
        DocumentSaveRequest documentSaveRequest, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await documentSaveRequest.Stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}