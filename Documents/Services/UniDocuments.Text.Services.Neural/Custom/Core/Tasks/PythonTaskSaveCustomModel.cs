using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Tasks;

[UseInPython]
public class SaveCustomModelInput
{
    public SaveCustomModelInput(string path, dynamic model)
    {
        Path = path;
        Model = model;
    }

    public string Path { get; }
    public dynamic Model { get; }
}

public class PythonTaskSaveCustomModel : PythonUnitTask<SaveCustomModelInput>
{
    public PythonTaskSaveCustomModel(SaveCustomModelInput input) : base(input) { }
    public override string ScriptName => "document_models";
    public override string MethodName => "save_custom";
}