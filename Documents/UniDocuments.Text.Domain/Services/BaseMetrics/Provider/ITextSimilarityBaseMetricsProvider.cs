namespace UniDocuments.Text.Domain.Services.BaseMetrics.Provider;

public interface ITextSimilarityBaseMetricsProvider
{
    ITextSimilarityBaseMetric GetBaseMetric(string name);
}