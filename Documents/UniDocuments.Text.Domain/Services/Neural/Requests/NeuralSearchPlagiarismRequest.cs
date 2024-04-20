namespace UniDocuments.Text.Domain.Services.Neural.Requests;

public record NeuralSearchPlagiarismRequest(Guid DocumentId, int TopN);