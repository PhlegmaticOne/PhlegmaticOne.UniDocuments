namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;

public record StorageSaveRequest(Guid Id, Stream Stream);
