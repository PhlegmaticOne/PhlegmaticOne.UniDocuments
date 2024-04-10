using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Domain.Services.Neural.Requests;

public record NeuralSearchPlagiarismRequest(Guid DocumentId, StreamContentReadResult Content, int TopN);