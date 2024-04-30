namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;

public record DocumentSaveRequest(Guid Id, string Name, Stream Stream);
