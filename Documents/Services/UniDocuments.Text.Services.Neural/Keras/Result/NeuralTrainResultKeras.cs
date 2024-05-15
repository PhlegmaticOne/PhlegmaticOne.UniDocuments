using UniDocuments.Text.Domain.Services.Neural.Models;

namespace UniDocuments.Text.Services.Neural.Keras.Result;

public class NeuralTrainResultKeras : NeuralModelTrainResult
{
    public int WindowSize { get; set; }
    public int BatchSize { get; set; }
}