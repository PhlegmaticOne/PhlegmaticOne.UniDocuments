namespace UniDocuments.Text.Domain.Services.DocumentsStorage.Requests;

public record DocumentSaveRequest(string Name, Stream Stream);
