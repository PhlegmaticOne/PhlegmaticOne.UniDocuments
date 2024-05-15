namespace UniDocuments.Text.Domain.Services.Neural.Options;

public class NeuralTrainOptionsBase
{
    public int EmbeddingSize { get; set; }
    public int Epochs { get; set; }
    public float LearningRate { get; set; }
}