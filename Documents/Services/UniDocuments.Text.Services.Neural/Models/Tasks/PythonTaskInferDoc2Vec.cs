using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Models.Tasks;

public class InferVectorInput
{
    public string Content { get; }
    public DocumentNeuralOptions Options { get; }
    public int TopN { get; }
    public dynamic Model { get; }

    public InferVectorInput(string content, DocumentNeuralOptions options, int topN, dynamic model)
    {
        Content = content;
        Options = options;
        TopN = topN;
        Model = model;
    }
}

public class InferVectorEntry
{
    public int ParagraphId { get; }
    public double Similarity { get; }

    public InferVectorEntry(int paragraphId, double similarity)
    {
        ParagraphId = paragraphId;
        Similarity = similarity;
    }
}

public class InferVectorOutput
{
    public InferVectorOutput(int paragraphId, List<InferVectorEntry> inferEntries)
    {
        ParagraphId = paragraphId;
        InferEntries = inferEntries;
    }

    public int ParagraphId { get; }
    public List<InferVectorEntry> InferEntries { get; }
}


public class PythonTaskInferDoc2Vec : PythonTask<InferVectorInput, InferVectorOutput>
{
    private readonly int _paragraphId;

    public PythonTaskInferDoc2Vec(InferVectorInput input, int paragraphId) : base(input)
    {
        _paragraphId = paragraphId;
    }

    public override string ScriptName => "doc2vec_model";
    public override string MethodName => "infer";
    protected override InferVectorOutput MapResult(dynamic result)
    {
        var entries = new List<InferVectorEntry>();
        
        foreach (var infer in result)
        {
            var paragraphId = int.Parse(((object)infer[0]).ToString()!);
            var similarity = double.Parse(((object)infer[1]).ToString()!);
            entries.Add(new InferVectorEntry(paragraphId, similarity));
        }
        
        return new InferVectorOutput(_paragraphId, entries);
    }
}