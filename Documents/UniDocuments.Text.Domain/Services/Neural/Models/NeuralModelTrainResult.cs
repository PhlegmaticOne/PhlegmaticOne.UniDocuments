namespace UniDocuments.Text.Domain.Services.Neural.Models;

[Serializable]
public class NeuralModelTrainResult
{
    public string Name { get; set; } = null!;
    public int EmbeddingSize { get; set; }
    public int Epochs { get; set; }
    public TimeSpan TrainTime { get; set; }
}