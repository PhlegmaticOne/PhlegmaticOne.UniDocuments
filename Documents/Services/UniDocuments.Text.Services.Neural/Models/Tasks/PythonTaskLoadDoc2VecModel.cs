using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Models.Tasks;

public class PythonTaskLoadDoc2VecModel : PythonTask<string, Doc2VecModel>
{
    public PythonTaskLoadDoc2VecModel(string input) : base(input) { }

    public override string ScriptName => "doc2vec_model";
    public override string MethodName => "load";
    protected override Doc2VecModel MapResult(dynamic result)
    {
        return new Doc2VecModel(result);
    }
}