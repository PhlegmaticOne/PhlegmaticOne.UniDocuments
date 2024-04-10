namespace UniDocuments.Text.Domain.Providers.Similarity.Requests;

public record DocumentsSimilarityRequest(
    Guid OriginalDocumentId,
    Guid ComparingDocumentId,
    List<string> Algorithms);