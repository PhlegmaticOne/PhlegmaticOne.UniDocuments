namespace UniDocuments.Text.Domain.Services.Similarity.Request;

public record UniDocumentsCompareRequest(
    Guid OriginalDocumentId,
    Guid ComparingDocumentId,
    List<string> Algorithms);