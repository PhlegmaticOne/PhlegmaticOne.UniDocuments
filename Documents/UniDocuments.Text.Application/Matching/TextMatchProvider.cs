using UniDocuments.Text.Domain.Providers.Matching;
using UniDocuments.Text.Domain.Providers.Matching.Requests;
using UniDocuments.Text.Domain.Providers.Matching.Responses;
using UniDocuments.Text.Domain.Services.Matching;
using UniDocuments.Text.Domain.Services.Matching.Options;

namespace UniDocuments.Text.Application.Matching;

public class TextMatchProvider : ITextMatchProvider
{
    private readonly ITextMatchingAlgorithm _matchingAlgorithm;
    private readonly IMatchingOptionsProvider _optionsProvider;

    public TextMatchProvider(ITextMatchingAlgorithm matchingAlgorithm, IMatchingOptionsProvider optionsProvider)
    {
        _matchingAlgorithm = matchingAlgorithm;
        _optionsProvider = optionsProvider;
    }
    
    public async Task<MatchTextsResponse> MatchAsync(MatchTextsRequest request, CancellationToken cancellationToken)
    {
        var response = new MatchTextsResponse(request.SourceText);
        var options = _optionsProvider.GetOptions();

        await Parallel.ForEachAsync(request.SuspiciousTexts, cancellationToken, (suspiciousText, token) =>
        {
            return ExecuteMatchTextAsync(request.SourceText, suspiciousText, response, options, token);
        });
        
        return response;
    }

    private async ValueTask ExecuteMatchTextAsync(string source, string suspicious,
        MatchTextsResponse response, MatchingOptions options, CancellationToken token)
    {
        var matchResult = await Task.Run(() => _matchingAlgorithm.Match(source, suspicious, options), token);

        lock (response)
        {
            response.AddResult(matchResult);
        }
    }
}