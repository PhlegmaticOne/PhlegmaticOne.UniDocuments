using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Options;

public class NeuralTrainOptionsKeras : NeuralTrainOptionsBase
{
    public int WindowSize { get; set; }
    public int BatchSize { get; set; }
}