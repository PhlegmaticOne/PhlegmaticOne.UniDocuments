using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.Neural.Models;
using UniDocuments.Text.Services.Neural.Doc2Vec.Options;
using UniDocuments.Text.Services.Neural.Doc2Vec.Tasks;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Models;

public class Doc2VecManagedModel
{
    private readonly dynamic _model;

    public Doc2VecManagedModel(dynamic model)
    {
        _model = model;
    }

    public Task<InferVectorOutput[]> InferDocumentAsync(UniDocumentContent content, int topN, Doc2VecOptions options)
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