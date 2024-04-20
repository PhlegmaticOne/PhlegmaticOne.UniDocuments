namespace UniDocuments.Text.Domain.Providers.Comparing.Requests;

public record CompareTextsRequest(
    string SourceText, string[] SuspiciousTexts, string BaseMetric);