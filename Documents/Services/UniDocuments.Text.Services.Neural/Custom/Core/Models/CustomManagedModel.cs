namespace UniDocuments.Text.Services.Neural.Custom.Core.Models;

public class CustomManagedModel
{
    private readonly dynamic _model;

    public CustomManagedModel(dynamic model)
    {
        _model = model;
    }
}