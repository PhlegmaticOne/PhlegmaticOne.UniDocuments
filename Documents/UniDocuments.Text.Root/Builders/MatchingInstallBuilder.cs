using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.Matching;
using UniDocuments.Text.Domain.Services.Matching.Options;

namespace UniDocuments.Text.Root.Builders;

public class MatchingInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public MatchingInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
    
    public void UseMatchingAlgorithm<T>() where T : class, ITextMatchingAlgorithm
    {
        _serviceCollection.AddSingleton<ITextMatchingAlgorithm, T>();
    }

    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IMatchingOptionsProvider
    {
        _serviceCollection.AddScoped<IMatchingOptionsProvider, T>();
        _serviceCollection.Configure<MatchingOptions>(configuration.GetSection(nameof(MatchingOptions)));
    }
}