namespace UniDocuments.App.Domain.Services.FileStorage;

public record FileSaveRequest(string FileName, Stream FileStream);
