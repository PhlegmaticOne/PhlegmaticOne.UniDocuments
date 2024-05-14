using UniDocuments.Text.Domain.Providers.BaseMetrics;
using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Services.BaseMetrics;

namespace UniDocuments.Text.Application.Comparing;

public class TextCompareProvider : ITextCompareProvider
{
    private readonly ITextSimilarityBaseMetricsProvider _baseMetricsProvider;

    public TextCompareProvider(ITextSimilarityBaseMetricsProvider baseMetricsProvider)
    {
        _baseMetricsProvider = baseMetricsProvider;
    }

    public async Task<CompareTextsResponse> CompareAsync(
        CompareTextsRequest request, CancellationToken cancellationToken)
    {
        var response = new CompareTextsResponse(request.SourceText);
        var baseMetric = _baseMetricsProvider.GetBaseMetric(request.BaseMetric);

        await Task.Run(() =>
        {
            foreach (var suspiciousText in request.SuspiciousTexts)
            {
                var result = Compare(request.SourceText, suspiciousText, baseMetric);
                response.AddResult(result);
            }
        }, cancellationToken);

        return response;
    }

    public CompareTextResult Compare(string a, string b, string metric)
    {
        var baseMetric = _baseMetricsProvider.GetBaseMetric(metric);
        var metricValue = baseMetric.Calculate(a, b);
        var isSuspicious = baseMetric.IsSuspicious(metricValue);
        return new CompareTextResult(a, metricValue, isSuspicious);
    }

    private static CompareTextResult Compare(
        string sourceText, string suspiciousText, ITextSimilarityBaseMetric baseMetric)
    {
        var metricValue = baseMetric.Calculate(sourceText, suspiciousText);
        var isSuspicious = baseMetric.IsSuspicious(metricValue);
        return new CompareTextResult(suspiciousText, metricValue, isSuspicious);
    }
}