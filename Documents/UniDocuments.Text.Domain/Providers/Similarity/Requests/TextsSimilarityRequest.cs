namespace UniDocuments.Text.Domain.Providers.Similarity.Requests;

public record TextsSimilarityRequest(string Text, string[] OtherTexts, string[] Algorithms);