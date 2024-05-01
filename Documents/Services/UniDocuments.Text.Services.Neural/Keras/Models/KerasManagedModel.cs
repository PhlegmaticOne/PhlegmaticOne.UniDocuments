using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Services.Neural.Common;
using UniDocuments.Text.Services.Neural.Keras.Options;
using UniDocuments.Text.Services.Neural.Keras.Tasks;

namespace UniDocuments.Text.Services.Neural.Keras.Models;

public class KerasManagedModel
{
    private readonly dynamic _model;

    public KerasManagedModel(dynamic model)
    {
        _model = model;
    }

    public async Task SaveAsync(string path)
    {
        var input = new SaveKerasModelInput(path, _model);
        await new PythonTaskSaveKerasModel(input);
    }
    
    public async Task<InferVectorOutput[]> InferDocumentAsync(UniDocumentContent content, int topN, KerasModelOptions options)
    {
        var input = new InferVectorInput(content.Paragraphs[0], options, topN, _model);
        var result = await new PythonTaskInferKerasModel(input, 0).Execute();
        return new[] { result! };
    }
}