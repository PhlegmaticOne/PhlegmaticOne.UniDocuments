using UniDocuments.App.Domain.Services.FileStorage;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Features.Text.Contracts;

namespace UniDocuments.Text.Features.Text.Services;

public class DocumentTextLoader : IDocumentTextLoader
{
    private readonly IFileStorage _fileStorage;
    private readonly IStreamContentReader _streamContentReader;

    public DocumentTextLoader(IFileStorage fileStorage, IStreamContentReader streamContentReader)
    {
        _fileStorage = fileStorage;
        _streamContentReader = streamContentReader;
    }
    
    public async Task<StreamContentReadResult> LoadTextAsync(Guid documentId, CancellationToken cancellationToken)
    {
        var request = new FileLoadRequest(documentId);
        var response = await _fileStorage.LoadAsync(request, cancellationToken);
        return await _streamContentReader.ReadAsync(response.FileStream!, cancellationToken);
    }
}