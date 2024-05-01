using PhlegmaticOne.PythonTasks;
using Python.Runtime;

namespace UniDocuments.Text.Services.Neural.Preprocessors.Tasks;

public class PreprocessTextInput
{
    public PreprocessTextInput(string text, string tokenizeRegex)
    {
        TokenizeRegex = tokenizeRegex;
        Text = text;
    }

    public string Text { get; }
    public string TokenizeRegex { get; }
}

public class PythonTaskPreprocessText : PythonTask<PreprocessTextInput, string>
{
    public PythonTaskPreprocessText(PreprocessTextInput input) : base(input) { }
    public override string ScriptName => "doc2vec";
    public override string MethodName => "preprocess_text";
    protected override string MapResult(dynamic result)
    {
        return ((object)result).ToPython().ToString();
    }
}