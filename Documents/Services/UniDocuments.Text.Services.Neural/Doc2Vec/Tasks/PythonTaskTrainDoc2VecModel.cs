using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Services.Neural.Doc2Vec.Models;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

[UseInPython]
public class TrainDoc2VecModelInput
{
    public IDocumentsTrainDatasetSource Source { get; }
    public Doc2VecOptions Options { get; }

    public TrainDoc2VecModelInput(IDocumentsTrainDatasetSource source, Doc2VecOptions options)
    {
        Source = source;
        Options = options;
    }
}

public class PythonTaskTrainDoc2VecModel : PythonTask<TrainDoc2VecModelInput, Doc2VecManagedModel>
{
    public PythonTaskTrainDoc2VecModel(TrainDoc2VecModelInput input) : base(input) { }
    public override string ScriptName => "doc2vec";
    public override string MethodName => "train";
    protected override Doc2VecManagedModel MapResult(dynamic result) => new Doc2VecManagedModel(result);
}