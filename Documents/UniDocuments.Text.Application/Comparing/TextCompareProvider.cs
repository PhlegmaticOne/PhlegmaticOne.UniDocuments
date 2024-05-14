using UniDocuments.Text.Domain.Providers.BaseMetrics;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Application.Comparing;

public class TextCompareProvider : ITextCompareProvider
{
    private readonly ITextSimilarityBaseMetricsProvider _baseMetricsProvider;

    public TextCompareProvider(ITextSimilarityBaseMetricsProvider baseMetricsProvider)
    {
        _baseMetricsProvider = baseMetricsProvider;
    }

    public CompareTextResult Compare(string a, string b, string metric)
    {
        var baseMetric = _baseMetricsProvider.GetBaseMetric(metric);
        var metricValue = baseMetric.Calculate(a, b);
        var isSuspicious = baseMetric.IsSuspicious(metricValue);
        return new CompareTextResult(a, metricValue, isSuspicious);
    }
}