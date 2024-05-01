using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Services.Neural.Common;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

public class PythonTaskInferDoc2Vec : PythonTask<InferVectorInput, InferVectorOutput>
{
    private readonly int _paragraphId;

    public PythonTaskInferDoc2Vec(InferVectorInput input, int paragraphId) : base(input)
    {
        _paragraphId = paragraphId;
    }

    public override string ScriptName => "doc2vec";
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