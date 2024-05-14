using System.Runtime.CompilerServices;
using UniDocuments.Text.Domain.Extensions;
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
            return new DocumentLoadResponse
            {
                Bytes = _fileContent,
                Name = _fileName
            };
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

    public ConfiguredCancelableAsyncEnumerable<DocumentLoadResponse> LoadAsync(IList<Guid> ids, CancellationToken cancellationToken)
    {
        return _fileContents
            .Where(x => ids.Contains(x.Key))
            .Select(x => x.Value.ToFileLoadResponse())
            .ToAsyncEnumerable()
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Guid> SaveAsync(StorageSaveRequest saveRequest, CancellationToken cancellationToken)
    {
        saveRequest.Stream.SeekToZero();
        var fileBytes = await GetFileContentAsync(saveRequest, cancellationToken);
        var fileId = saveRequest.Id;
        _fileContents[fileId] = new FileLoadResponsePrepared(saveRequest.Name, fileBytes);
        return saveRequest.Id;
    }

    private static async Task<byte[]> GetFileContentAsync(
        StorageSaveRequest storageSaveRequest, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await storageSaveRequest.Stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}