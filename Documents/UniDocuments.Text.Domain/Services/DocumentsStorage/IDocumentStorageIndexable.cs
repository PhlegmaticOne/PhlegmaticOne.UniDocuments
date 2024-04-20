using UniDocuments.Text.Domain.Services.DocumentsStorage.Responses;

namespace UniDocuments.Text.Domain.Services.DocumentsStorage;

public interface IDocumentStorageIndexable
{
    int StorageSize { get; }
    DocumentLoadResponse Load(int documentIndex);
}