using UniDocuments.App.Domain.FileStorage;

namespace UniDocuments.App.Data.Files.InMemory;

public class FileStorageInMemory : IFileStorage
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

        public FileLoadResponse ToFileLoadResponse()
        {
            var stream = new MemoryStream(_fileContent);
            return new FileLoadResponse(_fileId, _fileName, stream);
        }
    }
    
    private readonly Dictionary<Guid, FileLoadResponsePrepared> _fileContents = new();

    public Task<FileLoadResponse> LoadAsync(FileLoadRequest loadRequest, CancellationToken cancellationToken)
    {
        var fileId = loadRequest.FileId;
        
        var result = _fileContents.TryGetValue(fileId, out var fileLoadResponsePrepared) ? 
            fileLoadResponsePrepared.ToFileLoadResponse() : 
            FileLoadResponse.NoContent();

        return Task.FromResult(result);
    }

    public async Task<FileSaveResponse> SaveAsync(FileSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        var fileBytes = await GetFileContentAsync(saveRequest, cancellationToken);
        var fileId = Guid.NewGuid();
        _fileContents[fileId] = new FileLoadResponsePrepared(fileId, saveRequest.FileName, fileBytes);
        return new FileSaveResponse(fileId);
    }

    private static async Task<byte[]> GetFileContentAsync(
        FileSaveRequest fileSaveRequest, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await fileSaveRequest.FileStream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}