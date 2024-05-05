using UniDocuments.Text.Domain.Providers.Comparing;
using UniDocuments.Text.Domain.Providers.Comparing.Requests;
using UniDocuments.Text.Domain.Providers.Comparing.Responses;
using UniDocuments.Text.Domain.Services.BaseMetrics;
using UniDocuments.Text.Domain.Services.BaseMetrics.Provider;
using UniDocuments.Text.Domain.Services.Matching;
using UniDocuments.Text.Domain.Services.Matching.Options;

namespace UniDocuments.Text.Application.Comparing;

public class TextCompareProvider : ITextCompareProvider
{
    private readonly ITextSimilarityBaseMetricsProvider _baseMetricsProvider;
    private readonly IMatchingOptionsProvider _matchingOptionsProvider;
    private readonly ITextMatchingAlgorithm _matchingAlgorithm;

    public TextCompareProvider(
        ITextSimilarityBaseMetricsProvider baseMetricsProvider, 
        IMatchingOptionsProvider matchingOptionsProvider,
        ITextMatchingAlgorithm matchingAlgorithm)
    {
        _baseMetricsProvider = baseMetricsProvider;
        _matchingOptionsProvider = matchingOptionsProvider;
        _matchingAlgorithm = matchingAlgorithm;
    }

    public CompareTextsResponse Compare(CompareTextsRequest request)
    {
        var response = new CompareTextsResponse(request.SourceText);
        var baseMetric = _baseMetricsProvider.GetBaseMetric(request.BaseMetric);
        var options = _matchingOptionsProvider.GetOptions();
        
        foreach (var suspiciousText in request.SuspiciousTexts)
        {
            ExecuteSimilarityCheck(request.SourceText, suspiciousText, baseMetric, options, response);
        }
        
        return response;
    }

    public async Task<CompareTextsResponse> CompareAsync(
        CompareTextsRequest request, CancellationToken cancellationToken)
    {
        var response = new CompareTextsResponse(request.SourceText);
        var baseMetric = _baseMetricsProvider.GetBaseMetric(request.BaseMetric);
        var options = _matchingOptionsProvider.GetOptions();

        await Task.Run(() =>
        {
            foreach (var suspiciousText in request.SuspiciousTexts)
            {
                ExecuteSimilarityCheck(request.SourceText, suspiciousText, baseMetric, options, response);
            }
        }, cancellationToken);

        return response;
    }

    private void ExecuteSimilarityCheck(
        string sourceText, string suspiciousText, 
        ITextSimilarityBaseMetric baseMetric, MatchingOptions options, CompareTextsResponse response)
    {
        var metricValue = baseMetric.Calculate(sourceText, suspiciousText);
        CompareTextResult result;
            
        if (baseMetric.IsSuspicious(metricValue))
        {
            var matching = _matchingAlgorithm.Match(sourceText, suspiciousText, options);
            result = new CompareTextResult(suspiciousText, metricValue, matching.MatchEntries);
        }
        else
        {
            result = CompareTextResult.NoSimilar(suspiciousText, metricValue);       
        }

        response.AddResult(result);
    }
}