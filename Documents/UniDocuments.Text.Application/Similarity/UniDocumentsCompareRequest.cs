namespace UniDocuments.Text.Application.Similarity;

public record UniDocumentsCompareRequest(
    Guid OriginalDocumentId,
    Guid ComparingDocumentId,
    List<string> Algorithms);