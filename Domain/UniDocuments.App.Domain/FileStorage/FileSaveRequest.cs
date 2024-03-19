namespace UniDocuments.App.Domain.FileStorage;

public record FileSaveRequest(string FileName, Stream FileStream);
