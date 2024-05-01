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
    public float Alpha { get; set; }
    public float MinAlpha { get; set; }
    public int Dm { get; set; }
    public int WorkersCount { get; set; }
    public int MinWordsCount { get; set; }
}