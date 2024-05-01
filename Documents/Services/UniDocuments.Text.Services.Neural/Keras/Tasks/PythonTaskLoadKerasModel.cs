using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Common;
using UniDocuments.Text.Services.Neural.Keras.Models;

namespace UniDocuments.Text.Services.Neural.Keras.Tasks;

public class LoadKerasModelInput
{
    public LoadKerasModelInput(string path, string name, object vocab, IInferOptions options)
    {
        Path = path;
        Name = name;
        Vocab = vocab;
        Options = options;
    }

    public string Path { get; }
    public string Name { get; }
    public object Vocab { get; }
    public IInferOptions Options { get; }
}

public class PythonTaskLoadKerasModel : PythonTask<LoadKerasModelInput, KerasManagedModel>
{
    public PythonTaskLoadKerasModel(LoadKerasModelInput input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "load";
    protected override KerasManagedModel MapResult(dynamic result)
    {
        return new KerasManagedModel(result);
    }
}