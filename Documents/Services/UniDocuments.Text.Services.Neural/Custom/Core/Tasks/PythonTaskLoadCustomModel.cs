using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Custom.Core.Models;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Tasks;

public class PythonTaskLoadCustomModel : PythonTask<string, CustomManagedModel>
{
    public PythonTaskLoadCustomModel(string input) : base(input) { }
    public override string ScriptName => "document_models";
    public override string MethodName => "load_custom";
    protected override CustomManagedModel MapResult(dynamic result)
    {
        return new CustomManagedModel(result);
    }
}