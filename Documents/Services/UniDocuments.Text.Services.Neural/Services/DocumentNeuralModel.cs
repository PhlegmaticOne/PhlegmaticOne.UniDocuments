using Python.Runtime;
using UniDocuments.Text.Domain.Services.Neural;

namespace UniDocuments.Text.Services.Neural.Services;

public class DocumentNeuralModel : IDocumentsNeuralModel
{
    private const string BasePath = @"C:\Users\lolol\Downloads\t\{0}.txt";
    private const string PythonScriptName = "document_model";

    private readonly IDocumentNeuralDataHandler _dataHandler;
    
    private dynamic _script = null!;
    private dynamic _model = null!;
    
    public DocumentNeuralModel(IDocumentNeuralDataHandler dataHandler)
    {
        _dataHandler = dataHandler;
        PythonEngine.Initialize();
        PythonEngine.BeginAllowThreads();
    }

    public Task LoadAsync(string path)
    {
        var loadPath = Path.Combine(path, "model.bin");
        
        using (Py.GIL())
        {
            _script = Py.Import(PythonScriptName);
            _model = _script.load(loadPath);
        }
        
        return Task.CompletedTask;
    }

    public Task TrainAsync(IDocumentsNeuralSource source)
    {
        using (Py.GIL())
        {
            _script = Py.Import(PythonScriptName);
            _model = _script.train(source, _dataHandler);
        }
        
        return Task.CompletedTask;
    }

    public async Task<string> FindSimilarAsync(string text)
    {
        int fileId;
        string inferText;
        
        using (Py.GIL())
        {
            var infer = _script.infer(_model, text);
            fileId = int.Parse(((object)infer[0][0]).ToString()!);
            inferText = ((object)infer).ToString()!;
        }

        var fileText = await File.ReadAllTextAsync(string.Format(BasePath, fileId));
        var result = (object)inferText + "\n" + fileText;
        return result;
    }

    public Task SaveAsync(string path)
    {
        var savePath = Path.Combine(path, "model.bin");
        
        using (Py.GIL())
        {
            _model.save(savePath);
        }
        
        return Task.CompletedTask;
    }
}