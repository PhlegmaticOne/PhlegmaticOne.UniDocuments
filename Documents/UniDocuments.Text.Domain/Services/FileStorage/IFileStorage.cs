using UniDocuments.Text.Domain.Services.FileStorage.Requests;
using UniDocuments.Text.Domain.Services.FileStorage.Responses;

namespace UniDocuments.Text.Domain.Services.FileStorage;

public interface IFileStorage
{
    Task<FileLoadResponse> LoadAsync(FileLoadRequest loadRequest, CancellationToken cancellationToken);
    Task<FileSaveResponse> SaveAsync(FileSaveRequest saveRequest, CancellationToken cancellationToken);
}