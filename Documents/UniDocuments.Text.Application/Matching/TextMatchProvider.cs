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
    
    public Task<MatchTextsResponse> MatchAsync(MatchTextsRequest request, CancellationToken cancellationToken)
    {
        var response = new MatchTextsResponse(request.SourceText);
        var options = _optionsProvider.GetOptions();

        Parallel.ForEach(request.SuspiciousTexts, suspiciousText =>
        {
            var matchResult = _matchingAlgorithm.Match(request.SourceText, suspiciousText, options);
            
            lock (response)
            {
                response.AddResult(matchResult);
            }
        });
        
        return Task.FromResult(response);
    }
}