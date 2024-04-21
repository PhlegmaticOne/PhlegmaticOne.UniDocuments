using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Services.Neural.Custom.Core.Models;
using UniDocuments.Text.Services.Neural.Custom.Core.Options;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Tasks;

[UseInPython]
public class TrainCustomModelInput
{
    public IDocumentsTrainDatasetSource Source { get; }
    public CustomModelOptions Options { get; }

    public TrainCustomModelInput(IDocumentsTrainDatasetSource source, CustomModelOptions options)
    {
        Source = source;
        Options = options;
    }
}

public class PythonTaskTrainCustomModel : PythonTask<TrainCustomModelInput, CustomManagedModel>
{
    public PythonTaskTrainCustomModel(TrainCustomModelInput input) : base(input) { }
    public override string ScriptName => "document_models";
    public override string MethodName => "train_custom";
    protected override CustomManagedModel MapResult(dynamic result) => new CustomManagedModel(result);
}