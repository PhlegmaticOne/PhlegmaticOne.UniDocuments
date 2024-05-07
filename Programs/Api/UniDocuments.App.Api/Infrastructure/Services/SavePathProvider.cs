using Microsoft.Extensions.Options;
using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.Text.Domain.Services.SavePath;

namespace UniDocuments.App.Api.Infrastructure.Services;

public class SavePathProvider : ISavePathProvider
{
    public SavePathProvider(IOptions<ApplicationConfiguration> options)
    {
        var path = options.Value.SavePath;
        SavePath = Path.Combine(Directory.GetCurrentDirectory(), path);
    }
    
    public string SavePath { get; }
}