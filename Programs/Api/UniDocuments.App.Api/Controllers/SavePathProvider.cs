using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.App.Api.Controllers;

public class SavePathProvider : ISavePathProvider
{
    public SavePathProvider()
    {
        SavePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Models");
    }
    
    public string SavePath { get; }
}