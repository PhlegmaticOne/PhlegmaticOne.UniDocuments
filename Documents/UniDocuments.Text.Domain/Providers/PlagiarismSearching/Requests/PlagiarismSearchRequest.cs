namespace UniDocuments.Text.Domain.Providers.PlagiarismSearching.Requests;

public record PlagiarismSearchRequest(Guid DocumentId, int NDocuments, string Algorithm);