namespace UniDocuments.Text.Domain.Providers.Matching.Requests;

public record MatchTextsRequest(string SourceText, string[] SuspiciousTexts);