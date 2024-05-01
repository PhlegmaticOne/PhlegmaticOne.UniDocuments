using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Root.Builders;

public class StreamContentReaderInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public StreamContentReaderInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    
    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, ITextProcessOptionsProvider
    {
        _serviceCollection.AddSingleton<ITextProcessOptionsProvider, T>();
        _serviceCollection.Configure<TextProcessOptions>(configuration.GetSection(nameof(TextProcessOptions)));
    }
    
    public void UseWordCountApproximator<T>() where T : class, IWordsCountApproximator
    {
        _serviceCollection.AddSingleton<IWordsCountApproximator, T>();
    }
}