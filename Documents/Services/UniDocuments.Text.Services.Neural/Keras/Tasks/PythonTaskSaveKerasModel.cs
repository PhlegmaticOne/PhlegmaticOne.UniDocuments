using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Keras.Tasks;

[UseInPython]
public class SaveKerasModelInput
{
    public SaveKerasModelInput(string path, string name, dynamic model)
    {
        Path = path;
        Name = name;
        Model = model;
    }

    public string Path { get; }
    public string Name { get; }
    public dynamic Model { get; }
}

public class PythonTaskSaveKerasModel : PythonUnitTask<SaveKerasModelInput>
{
    public PythonTaskSaveKerasModel(SaveKerasModelInput input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "save";
}