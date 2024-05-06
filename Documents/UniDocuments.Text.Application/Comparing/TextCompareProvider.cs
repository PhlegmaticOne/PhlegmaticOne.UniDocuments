using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;

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

    private static CompareTextResult Compare(
        string sourceText, string suspiciousText, ITextSimilarityBaseMetric baseMetric)
    {
        var metricValue = baseMetric.Calculate(sourceText, suspiciousText);
        var isSuspicious = baseMetric.IsSuspicious(metricValue);
        return new CompareTextResult(suspiciousText, metricValue, isSuspicious);
    }
}