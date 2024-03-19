namespace UniDocuments.App.Domain.FileStorage;

public interface IFileStorage
{
    Task<FileLoadResponse> LoadAsync(FileLoadRequest loadRequest, CancellationToken cancellationToken);
    Task<FileSaveResponse> SaveAsync(FileSaveRequest saveRequest, CancellationToken cancellationToken);
}