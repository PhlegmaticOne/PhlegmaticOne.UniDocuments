using UniDocuments.Text.Domain.Services.BaseMetrics;

namespace UniDocuments.Text.Domain.Providers.BaseMetrics;

public interface ITextSimilarityBaseMetricsProvider
{
    ITextSimilarityBaseMetric GetBaseMetric(string name);
}