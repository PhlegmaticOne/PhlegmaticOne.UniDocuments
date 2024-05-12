namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;

public record StorageSaveRequest(Guid Id, string Name, Stream Stream);
