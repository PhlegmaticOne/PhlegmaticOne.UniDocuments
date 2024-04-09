namespace UniDocuments.Text.Domain.Services.Searching.Request;

public record PlagiarismSearchRequest(Guid DocumentId, int NDocuments);