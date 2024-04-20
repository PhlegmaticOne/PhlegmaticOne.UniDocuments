using Python.Runtime;
using UniDocuments.Text.Domain.Providers.PlagiarismSearching.Responses;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.Neural.Requests;

namespace UniDocuments.Text.Services.Neural.Models;

public class DocumentNeuralModel : IDocumentsNeuralModel
{
    private const string PythonScriptName = "document_model";
    private const string ModelName = "model.bin";

    private readonly IDocumentsNeuralDataHandler _dataHandler;
    private readonly IDocumentNeuralOptionsProvider _optionsProvider;

    private dynamic _script = null!;
    private dynamic _model = null!;
    
    public DocumentNeuralModel(IDocumentsNeuralDataHandler dataHandler, IDocumentNeuralOptionsProvider optionsProvider)
    {
        _dataHandler = dataHandler;
        _optionsProvider = optionsProvider;
        PythonEngine.Initialize();
        PythonEngine.BeginAllowThreads();
    }

    public Task LoadAsync(string path, CancellationToken cancellationToken)
    {
        var loadPath = Path.Combine(path, ModelName);
        
        using (Py.GIL())
        {
            _script = Py.Import(PythonScriptName);
            _model = _script.load(loadPath);
        }
        
        return Task.CompletedTask;
    }

    public Task TrainAsync(IDocumentsNeuralSource source, CancellationToken cancellationToken)
    {
        var options = _optionsProvider.GetOptions();

        using (Py.GIL())
        {
            _script = Py.Import(PythonScriptName);
            _model = _script.train(source, options, _dataHandler);
        }
        
        return Task.CompletedTask;
    }

    public Task<List<ParagraphPlagiarismData>> FindSimilarAsync(
        NeuralSearchPlagiarismRequest request, CancellationToken cancellationToken)
    {
        var result = new List<ParagraphPlagiarismData>();
        var options = _optionsProvider.GetOptions();

        // using (Py.GIL())
        // {
        //     foreach (var rawParagraph in request.Content.Paragraphs)
        //     {
        //         var text = rawParagraph.Content;
        //         var infer = _script.infer(_model, options, text, request.TopN);
        //
        //         if (((object)infer).ToPython().IsNone())
        //         {
        //             continue;
        //         }
        //
        //         var suspiciousParagraphs = SelectSuspiciousParagraphs(infer, request.DocumentId);
        //         result.Add(new ParagraphPlagiarismData(rawParagraph.Id, suspiciousParagraphs));
        //     }
        // }
        
        return Task.FromResult(result);
    }

    public Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        var savePath = Path.Combine(path, ModelName);
        
        using (Py.GIL())
        {
            _model.save(savePath);
        }
        
        return Task.CompletedTask;
    }

    private List<ParagraphSearchData> SelectSuspiciousParagraphs(dynamic infer, Guid documentId)
    {
        var suspiciousParagraphs = new List<ParagraphSearchData>();

        foreach (var inferData in infer)
        {
            var pythonModelId = int.Parse(((object)inferData[0]).ToString()!);
            var modelSimilarity = float.Parse(((object)inferData[1]).ToString()!);
            var saveData = _dataHandler.GetSaveData(pythonModelId);

            if (saveData.DocumentId == documentId)
            {
                continue;
            }
                    
            suspiciousParagraphs.Add(new ParagraphSearchData
            {
                Similarity = modelSimilarity,
                DocumentId = saveData.DocumentId,
                DocumentName = saveData.DocumentName,
                OriginalId = saveData.OriginalId
            });
        }

        return suspiciousParagraphs;
    }
}