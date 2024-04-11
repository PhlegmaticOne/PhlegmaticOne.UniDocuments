using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Plagiarism.Matching.Algorithm.Services;

namespace UniDocuments.Text.Root.Builders.Algorithms;

public class MatchingInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public MatchingInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IMatchingOptionsProvider
    {
        _serviceCollection.AddScoped<IMatchingOptionsProvider, T>();
        _serviceCollection.Configure<MatchingOptions>(configuration.GetSection(nameof(MatchingOptions)));
    }
}