namespace UniDocuments.Text.Domain.Services.BaseMetrics;

public interface ITextSimilarityBaseMetric
{
    string Name { get; }
    double Calculate(string sourceText, string suspiciousText);
    bool IsSuspicious(double metricValue);
}