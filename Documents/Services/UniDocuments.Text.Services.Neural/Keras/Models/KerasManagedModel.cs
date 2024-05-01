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

    public async Task SaveAsync(string path, string name)
    {
        var input = new SaveKerasModelInput(path, name, _model);
        await new PythonTaskSaveKerasModel(input);
    }
    
    public Task<InferVectorOutput[]> InferDocumentAsync(UniDocumentContent content, int topN, KerasModelOptions options)
    {
        var tasks = new Task<InferVectorOutput>[content.ParagraphsCount];

        for (var i = 0; i < content.ParagraphsCount; i++)
        {
            var input = new InferVectorInput(content.Paragraphs[i], options, topN, _model);
            var inferTask = new PythonTaskInferKerasModel(input, i);
            tasks[i] = inferTask.Execute()!;
        }

        return Task.WhenAll(tasks);
    }
}