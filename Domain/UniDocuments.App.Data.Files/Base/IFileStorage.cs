using UniDocuments.App.Data.Files.Models;

namespace UniDocuments.App.Data.Files.Base;

public interface IFileStorage
{
    Task<IList<FileLoadResponse>> GetFilesPagedAsync(int pageIndex, int pageSize);
    Task<FileLoadResponse> LoadFileAsync(Guid fileId);
    Task SaveFileAsync(FileLocalSaveRequest fileLocalSaveRequest);
}