using PhlegmaticOne.PythonTasks;

namespace UniDocuments.Text.Services.Neural.Vocab.Tasks;

public class PythonTaskLoadVocab : PythonTask<string, dynamic>
{
    public PythonTaskLoadVocab(string input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "load_vocab";
    protected override dynamic MapResult(dynamic result) => result;
}