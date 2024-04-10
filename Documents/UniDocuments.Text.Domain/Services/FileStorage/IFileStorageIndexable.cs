using UniDocuments.Text.Domain.Services.FileStorage.Responses;

namespace UniDocuments.Text.Domain.Services.FileStorage;

public interface IFileStorageIndexable
{
    int StorageSize { get; }
    FileLoadResponse Load(int documentIndex);
}