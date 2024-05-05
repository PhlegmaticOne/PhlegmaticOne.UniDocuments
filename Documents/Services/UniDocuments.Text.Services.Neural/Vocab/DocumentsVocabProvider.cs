using System.Diagnostics;
using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Vocab;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Services.StreamReading.Options;
using UniDocuments.Text.Services.Neural.Vocab.Tasks;

namespace UniDocuments.Text.Services.Neural.Vocab;

public class DocumentsVocabProvider : IDocumentsVocabProvider
{
    private readonly ITextProcessOptionsProvider _optionsProvider;
    private readonly ISavePathProvider _savePathProvider;

    private dynamic _kerasVocab = null!;

    public DocumentsVocabProvider(
        ITextProcessOptionsProvider optionsProvider,
        ISavePathProvider savePathProvider)
    {
        _optionsProvider = optionsProvider;
        _savePathProvider = savePathProvider;
    }

    public bool IsLoaded { get; private set; }

    public async Task<DocumentVocabData> BuildAsync(IDocumentsTrainDatasetSource source, CancellationToken cancellationToken)
    {
        var input = new BuildVocabInput(_savePathProvider.SavePath, source, _optionsProvider.GetOptions());
        var timer = Stopwatch.StartNew();
        _kerasVocab = (await new PythonTaskBuildVocab(input))!;
        timer.Stop();
        IsLoaded = true;
        
        return new DocumentVocabData
        {
            BuildTime = timer.Elapsed,
            DocumentsCount = _kerasVocab.documents_count,
            VocabSize = _kerasVocab.vocab_size
        };
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        _kerasVocab = (await new PythonTaskLoadVocab(_savePathProvider.SavePath))!;
        IsLoaded = true;
    }

    public object GetVocab()
    {
        return _kerasVocab;
    }
}