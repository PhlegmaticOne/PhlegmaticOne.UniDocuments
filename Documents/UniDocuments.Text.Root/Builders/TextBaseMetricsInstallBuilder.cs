using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.BaseMetrics;

namespace UniDocuments.Text.Root.Builders;

public class TextBaseMetricsInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public TextBaseMetricsInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseBaseMetric<T>() where T : class, ITextSimilarityBaseMetric
    {
        _serviceCollection.AddSingleton<ITextSimilarityBaseMetric, T>();
    }
}