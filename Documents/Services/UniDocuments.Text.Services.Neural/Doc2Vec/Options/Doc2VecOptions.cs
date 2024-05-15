using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Services.Neural.Common;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Options;

[UseInPython]
public class Doc2VecOptions : INeuralOptions, IInferOptions
{
    public string Name { get; set; } = null!;
    public string TokenizeRegex { get; set; } = null!;
    public int EmbeddingSize { get; set; }
    public int Epochs { get; set; }
    public int MaxInferEpochs { get; set; }
    public int DefaultInferEpochs { get; set; }
    public float Alpha { get; set; }
    public float MinAlpha { get; set; }
    public int Dm { get; set; }
    public int WorkersCount { get; set; }
    public int MinWordsCount { get; set; }

    public int GetInferEpochs(int epochs)
    {
        return epochs <= 0 ? DefaultInferEpochs : Math.Min(epochs, MaxInferEpochs);
    }

    public Doc2VecOptions Merge(NeuralTrainOptionsDoc2Vec optionsDoc2Vec)
    {
        return new Doc2VecOptions
        {
            Alpha = optionsDoc2Vec.LearningRate == 0 ? Alpha : optionsDoc2Vec.LearningRate,
            MinAlpha = optionsDoc2Vec.MinAlpha == 0 ? MinAlpha : optionsDoc2Vec.MinAlpha,
            MaxInferEpochs = MaxInferEpochs,
            Dm = optionsDoc2Vec.Dm,
            Epochs = optionsDoc2Vec.Epochs == 0 ? Epochs : optionsDoc2Vec.Epochs,
            EmbeddingSize = optionsDoc2Vec.EmbeddingSize == 0 ? EmbeddingSize : optionsDoc2Vec.EmbeddingSize,
            TokenizeRegex = TokenizeRegex,
            WorkersCount = optionsDoc2Vec.WorkersCount == 0 ? WorkersCount : optionsDoc2Vec.WorkersCount,
            Name = Name,
            DefaultInferEpochs = DefaultInferEpochs,
            MinWordsCount = optionsDoc2Vec.MinWordsCount == 0 ? MinWordsCount : optionsDoc2Vec.MinWordsCount,
        };
    }
}