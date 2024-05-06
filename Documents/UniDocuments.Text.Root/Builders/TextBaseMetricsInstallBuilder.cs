using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Options;

namespace UniDocuments.Text.Root.Builders;

public class TextBaseMetricsInstallBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public TextBaseMetricsInstallBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseOptionsProvider<T>(IConfiguration configuration) where T : class, IMetricBaselinesOptionsProvider
    {
        _serviceCollection.AddSingleton<IMetricBaselinesOptionsProvider, T>();
        _serviceCollection.Configure<MetricBaselines>(configuration.GetSection(nameof(MetricBaselines)));
    }

    public void UseBaseMetric<T>() where T : class, ITextSimilarityBaseMetric
    {
        _serviceCollection.AddSingleton<ITextSimilarityBaseMetric, T>();
    }
}