using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Matching.Options;

namespace UniDocuments.Text.Services.Matching.Options;

public class MatchingOptionsProvider : IMatchingOptionsProvider
{
    private readonly IOptionsSnapshot<MatchingOptions> _optionsSnapshot;

    public MatchingOptionsProvider(IOptionsSnapshot<MatchingOptions> optionsSnapshot)
    {
        _optionsSnapshot = optionsSnapshot;
    }
    
    public MatchingOptions GetOptions()
    {
        return _optionsSnapshot.Value;
    }
}