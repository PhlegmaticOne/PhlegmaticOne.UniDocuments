using System.Reflection;

namespace UniDocuments.Text.Domain.Services.BaseMetrics.Provider;

public class TextSimilarityBaseMetricsProvider : ITextSimilarityBaseMetricsProvider
{
    private readonly Dictionary<string, ITextSimilarityBaseMetric> _baseMetrics;
    private readonly ITextSimilarityBaseMetric _defaultMetric;
    
    public TextSimilarityBaseMetricsProvider(IEnumerable<ITextSimilarityBaseMetric> baseMetrics)
    {
        var metrics = baseMetrics.ToArray();
        _baseMetrics = metrics.ToDictionary(x => x.Name, x => x);
        _defaultMetric = GetDefaultMetric(metrics);
    }
    
    public ITextSimilarityBaseMetric GetBaseMetric(string name)
    {
        return _baseMetrics.GetValueOrDefault(name, _defaultMetric);
    }

    private static ITextSimilarityBaseMetric GetDefaultMetric(IEnumerable<ITextSimilarityBaseMetric> baseMetrics)
    {
        return baseMetrics.First(x => x.GetType().GetCustomAttribute(typeof(BaseMetricDefaultAttribute)) is not null);
    }
}