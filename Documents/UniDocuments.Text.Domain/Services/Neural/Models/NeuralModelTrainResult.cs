namespace UniDocuments.Text.Domain.Services.Neural.Models;

[Serializable]
public class NeuralModelTrainResult
{
    public string Name { get; set; } = null!;
    public int EmbeddingSize { get; set; }
    public int Epochs { get; set; }
    public float LearningRate { get; set; }
    public TimeSpan TrainTime { get; set; }
    public string? ErrorMessage { get; set; }

    public bool IsError()
    {
        return !string.IsNullOrEmpty(ErrorMessage);
    }
}