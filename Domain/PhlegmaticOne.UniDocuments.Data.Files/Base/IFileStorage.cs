using PhlegmaticOne.UniDocuments.Data.Files.Models;

namespace PhlegmaticOne.UniDocuments.Data.Files.Base;

public interface IFileStorage
{
    Task<IList<FileLoadResponse>> GetFilesPagedAsync(int pageIndex, int pageSize);
    Task<FileLoadResponse> LoadFileAsync(Guid fileId);
    Task SaveFileAsync(FileLocalSaveRequest fileLocalSaveRequest);
}