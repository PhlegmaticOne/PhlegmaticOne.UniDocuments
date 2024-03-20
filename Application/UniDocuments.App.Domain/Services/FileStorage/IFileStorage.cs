namespace UniDocuments.App.Domain.Services.FileStorage;

public interface IFileStorage
{
    Task<FileLoadResponse> LoadAsync(FileLoadRequest loadRequest, CancellationToken cancellationToken);
    Task<FileSaveResponse> SaveAsync(FileSaveRequest saveRequest, CancellationToken cancellationToken);
}