using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Models.Tasks;

public class TrainDoc2VecModelInput
{
    public IDocumentsNeuralSource Source { get; }
    public DocumentNeuralOptions Options { get; }

    public TrainDoc2VecModelInput(IDocumentsNeuralSource source, DocumentNeuralOptions options)
    {
        Source = source;
        Options = options;
    }
}

public class PythonTaskTrainDoc2VecModel : PythonTask<TrainDoc2VecModelInput, Doc2VecModel>
{
    public PythonTaskTrainDoc2VecModel(TrainDoc2VecModelInput input) : base(input)
    {
    }

    public override string ScriptName => "doc2vec_model";
    public override string MethodName => "train";
    protected override Doc2VecModel MapResult(dynamic result)
    {
        return new Doc2VecModel(result);
    }
}