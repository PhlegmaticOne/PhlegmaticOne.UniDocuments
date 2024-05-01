using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.App.Api.Infrastructure.Services;

public class SavePathProvider : ISavePathProvider
{
    public SavePathProvider()
    {
        SavePath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Models");
    }
    
    public string SavePath { get; }
}