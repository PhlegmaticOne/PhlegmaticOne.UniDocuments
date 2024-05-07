using System.Reflection;
using UniDocuments.Text.Domain.Providers.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Attributes;

namespace UniDocuments.Text.Application.BaseMetrics;

public class TextSimilarityBaseMetricsProvider : ITextSimilarityBaseMetricsProvider
{
    private readonly Dictionary<string, ITextSimilarityBaseMetric> _baseMetrics;
    private readonly ITextSimilarityBaseMetric _defaultMetric;
    
    public TextSimilarityBaseMetricsProvider(IEnumerable<ITextSimilarityBaseMetric> baseMetrics)
    {
        var metrics = baseMetrics.ToArray();
        _baseMetrics = metrics.ToDictionary(x => x.Name.ToLower(), x => x);
        _defaultMetric = GetDefaultMetric(metrics);
    }
    
    public ITextSimilarityBaseMetric GetBaseMetric(string name)
    {
        return _baseMetrics.GetValueOrDefault(name.ToLower(), _defaultMetric);
    }

    private static ITextSimilarityBaseMetric GetDefaultMetric(IEnumerable<ITextSimilarityBaseMetric> baseMetrics)
    {
        return baseMetrics.First(x => x.GetType().GetCustomAttribute(typeof(BaseMetricDefaultAttribute)) is not null);
    }
}