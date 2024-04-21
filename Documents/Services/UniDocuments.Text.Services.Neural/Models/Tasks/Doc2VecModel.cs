using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Neural.Options;

namespace UniDocuments.Text.Services.Neural.Models.Tasks;

public class Doc2VecModel
{
    private readonly dynamic _model;

    public Doc2VecModel(dynamic model)
    {
        _model = model;
    }

    public Task<InferVectorOutput[]> InferDocumentAsync(UniDocumentContent content, int topN, DocumentNeuralOptions options)
    {
        var tasks = new Task<InferVectorOutput>[content.ParagraphsCount];

        for (var i = 0; i < content.ParagraphsCount; i++)
        {
            var input = new InferVectorInput(content.Paragraphs[i], options, topN, _model);
            var inferTask = new PythonTaskInferDoc2Vec(input, i);
            tasks[i] = inferTask.Execute()!;
        }

        return Task.WhenAll(tasks);
    }

    public async Task SaveAsync(string path)
    {
        var input = new SaveDoc2VecModelInput(path, _model);
        await new PythonTaskSaveDoc2VecModel(input);
    }
}