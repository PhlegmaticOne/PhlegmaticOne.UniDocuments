using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Vocab.Tasks;

[UseInPython]
public class BuildVocabInput
{
    public BuildVocabInput(string basePath, IDocumentsTrainDatasetSource source, TextProcessOptions options)
    {
        BasePath = basePath;
        Source = source;
        Options = options;
    }

    public string BasePath { get; }
    public IDocumentsTrainDatasetSource Source { get; }
    public TextProcessOptions Options { get; }
}

public class PythonTaskBuildVocab : PythonTask<BuildVocabInput, dynamic>
{
    public PythonTaskBuildVocab(BuildVocabInput input) : base(input) { }
    public override string ScriptName => "keras2vec";
    public override string MethodName => "build_vocab";
    protected override dynamic MapResult(dynamic result) => result;
}