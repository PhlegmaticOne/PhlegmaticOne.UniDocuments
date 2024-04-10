namespace UniDocuments.Text.Domain.Services.FileStorage.Requests;

public record FileSaveRequest(string FileName, Stream FileStream);
