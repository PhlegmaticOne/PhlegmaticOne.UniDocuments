using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Options;

public class NeuralTrainOptionsDoc2Vec : NeuralTrainOptionsBase
{
    public float MinAlpha { get; set; }
    public int Dm { get; set; }
    public int WorkersCount { get; set; }
    public int MinWordsCount { get; set; }
}