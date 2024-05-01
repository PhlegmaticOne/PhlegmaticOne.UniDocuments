namespace UniDocuments.Text.Services.Neural.Common;

public class PythonModelPathData
{
    public PythonModelPathData(string basePath, string name)
    {
        BasePath = basePath;
        Name = name;
    }

    public string BasePath { get; }
    public string Name { get; }
}