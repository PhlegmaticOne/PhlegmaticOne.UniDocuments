using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Common;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

[UseInPython]
public class SaveDoc2VecModelInput
{
    public SaveDoc2VecModelInput(PythonModelPathData path, dynamic model)
    {
        Path = path;
        Model = model;
    }

    public PythonModelPathData Path { get; }
    public dynamic Model { get; }
}

public class PythonTaskSaveDoc2VecModel : PythonUnitTask<SaveDoc2VecModelInput>
{
    public PythonTaskSaveDoc2VecModel(SaveDoc2VecModelInput input) : base(input) { }
    public override string ScriptName => "doc2vec";
    public override string MethodName => "save";
}