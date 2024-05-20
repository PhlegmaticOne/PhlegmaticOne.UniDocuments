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
    public int MaxInferEpochs { get; set; }
    public int DefaultInferEpochs { get; set; }
    public double LearningRate { get; set; }
    public double BaseLine { get; set; }
    public bool IsPlotResults { get; set; }
    public int Verbose { get; set; }
    public string Loss { get; set; } = null!;
    public string[] Metrics { get; set; } = null!;
    public List<KerasLayerConfiguration> Layers { get; set; } = null!;
    
    public int GetInferEpochs(int epochs)
    {
        return epochs <= 0 ? DefaultInferEpochs : Math.Min(epochs, MaxInferEpochs);
    }

    public KerasModelOptions Merge(NeuralTrainOptionsKeras optionsKeras)
    {
        return new KerasModelOptions
        {
            LearningRate = optionsKeras.LearningRate == 0 ? LearningRate : optionsKeras.LearningRate,
            EmbeddingSize = optionsKeras.EmbeddingSize == 0 ? EmbeddingSize : optionsKeras.EmbeddingSize,
            Epochs = optionsKeras.Epochs == 0 ? Epochs : optionsKeras.Epochs,
            MaxInferEpochs = MaxInferEpochs,
            TokenizeRegex = TokenizeRegex,
            Name = Name,
            Layers = Layers,
            DefaultInferEpochs = DefaultInferEpochs,
            Loss = Loss,
            Metrics = Metrics,
            BatchSize = optionsKeras.BatchSize == 0 ? BatchSize : optionsKeras.BatchSize,
            WindowSize = optionsKeras.WindowSize == 0 ? WindowSize : optionsKeras.WindowSize,
            IsPlotResults = IsPlotResults,
            Verbose = Verbose
        };
    }
}