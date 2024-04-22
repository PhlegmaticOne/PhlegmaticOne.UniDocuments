using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Services.Neural.Custom.Core.Tasks;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Models;

public class CustomManagedModel
{
    private readonly dynamic _model;

    public CustomManagedModel(dynamic model)
    {
        _model = model;
    }

    public async Task SaveAsync(string path)
    {
        var input = new SaveCustomModelInput(path, _model);
        await new PythonTaskSaveCustomModel(input);
    }
}