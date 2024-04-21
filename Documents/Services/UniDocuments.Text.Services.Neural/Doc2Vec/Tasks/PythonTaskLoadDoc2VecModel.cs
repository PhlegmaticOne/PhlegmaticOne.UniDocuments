using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Doc2Vec.Models;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

public class PythonTaskLoadDoc2VecModel : PythonTask<string, Doc2VecManagedModel>
{
    public PythonTaskLoadDoc2VecModel(string input) : base(input) { }
    public override string ScriptName => "document_models";
    public override string MethodName => "load_doc2vec";
    protected override Doc2VecManagedModel MapResult(dynamic result) => new Doc2VecManagedModel(result);
}