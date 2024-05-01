using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Common;

namespace UniDocuments.Text.Services.Neural.Keras.Options;

[UseInPython]
public class KerasModelOptions : INeuralOptions, IInferOptions
{
    public string Name { get; set; } = null!;
    public string TokenizeRegex { get; set; } = null!;
    public int EmbeddingSize { get; set; }
    public int WindowSize { get; set; }
    public int BatchSize { get; set; }
    public int Epochs { get; set; }
    public int InferEpochs { get; set; }
    public double LearningRate { get; set; }
    public int Verbose { get; set; }
    public string Loss { get; set; } = null!;
    public string[] Metrics { get; set; } = null!;
    public List<KerasLayerConfiguration> Layers { get; set; } = null!;
}