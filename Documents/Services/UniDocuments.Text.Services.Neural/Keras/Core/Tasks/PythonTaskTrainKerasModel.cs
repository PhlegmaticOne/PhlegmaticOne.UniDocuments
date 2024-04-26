using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Services.Neural.Keras.Core.Models;
using UniDocuments.Text.Services.Neural.Keras.Core.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Core.Tasks;

[UseInPython]
public class TrainKerasModelInput
{
    public IDocumentsTrainDatasetSource Source { get; }
    public KerasModelOptions Options { get; }

    public TrainKerasModelInput(IDocumentsTrainDatasetSource source, KerasModelOptions options)
    {
        Source = source;
        Options = options;
    }
}

public class PythonTaskTrainKerasModel : PythonTask<TrainKerasModelInput, KerasManagedModel>
{
    public PythonTaskTrainKerasModel(TrainKerasModelInput input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "train";
    protected override KerasManagedModel MapResult(dynamic result) => new KerasManagedModel(result);
}