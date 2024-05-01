﻿using System.Diagnostics;
using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Vocab;
using UniDocuments.Text.Domain.Services.SavePath;
using UniDocuments.Text.Domain.Services.StreamReading.Options;
using UniDocuments.Text.Services.Neural.Vocab.Tasks;

namespace UniDocuments.Text.Services.Neural.Vocab;

public class DocumentsVocabProvider : IDocumentsVocabProvider
{
    private readonly IDocumentsTrainDatasetSource _source;
    private readonly ITextProcessOptionsProvider _optionsProvider;
    private readonly ISavePathProvider _savePathProvider;

    private dynamic _kerasVocab = null!;

    public DocumentsVocabProvider(
        IDocumentsTrainDatasetSource source,
        ITextProcessOptionsProvider optionsProvider,
        ISavePathProvider savePathProvider)
    {
        _source = source;
        _optionsProvider = optionsProvider;
        _savePathProvider = savePathProvider;
    }
    
    public async Task<DocumentVocabData> BuildAsync(CancellationToken cancellationToken)
    {
        var input = new BuildVocabInput(_savePathProvider.SavePath, _source, _optionsProvider.GetOptions());
        var timer = Stopwatch.StartNew();
        _kerasVocab = (await new PythonTaskBuildVocab(input))!;
        timer.Stop();

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
    }

    public object GetVocab()
    {
        return _kerasVocab;
    }
}