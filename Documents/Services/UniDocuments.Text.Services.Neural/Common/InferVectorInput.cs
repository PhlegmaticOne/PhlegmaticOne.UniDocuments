using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Common;

[UseInPython]
public class InferVectorInput
{
    public string Content { get; }
    public IInferOptions Options { get; }
    public int TopN { get; }
    public int InferEpochs { get; }
    public dynamic Model { get; }

    public InferVectorInput(string content, IInferOptions options, int topN, int inferEpochs, dynamic model)
    {
        Content = content;
        Options = options;
        TopN = topN;
        InferEpochs = inferEpochs;
        Model = model;
    }
}