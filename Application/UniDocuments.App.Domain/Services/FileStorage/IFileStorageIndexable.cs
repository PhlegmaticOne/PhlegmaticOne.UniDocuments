namespace UniDocuments.App.Domain.Services.FileStorage;

public interface IFileStorageIndexable
{
    int StorageSize { get; }
    FileLoadResponse Load(int documentIndex);
}