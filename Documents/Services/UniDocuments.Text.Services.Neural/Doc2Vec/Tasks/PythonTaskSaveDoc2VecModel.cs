using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

[UseInPython]
public class SaveDoc2VecModelInput
{
    public SaveDoc2VecModelInput(string path, dynamic model)
    {
        Path = path;
        Model = model;
    }

    public string Path { get; }
    public dynamic Model { get; }
}

public class PythonTaskSaveDoc2VecModel : PythonUnitTask<SaveDoc2VecModelInput>
{
    public PythonTaskSaveDoc2VecModel(SaveDoc2VecModelInput input) : base(input) { }
    public override string ScriptName => "document_models";
    public override string MethodName => "save_doc2vec";
}