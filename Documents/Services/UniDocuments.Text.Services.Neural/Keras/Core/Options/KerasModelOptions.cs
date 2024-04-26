using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Core.Options;

[UseInPython]
public abstract class KerasModelOptions : INeuralOptions, IInferOptions
{
    public string Name { get; set; } = null!;
    public string TokenizeRegex { get; set; } = null!;
    public int EmbeddingSize { get; set; }
    public int WindowSize { get; set; }
    public int BatchSize { get; set; }
    public int Epochs { get; set; }
    public double LearningRate { get; set; }
    public int Verbose { get; set; }
    public List<KerasLayerConfiguration> Layers { get; set; } = null!;
}