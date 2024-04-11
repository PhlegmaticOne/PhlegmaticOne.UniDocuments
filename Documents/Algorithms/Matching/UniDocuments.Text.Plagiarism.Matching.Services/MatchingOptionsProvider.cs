using Microsoft.Extensions.Options;
using UniDocuments.Text.Plagiarism.Matching.Algorithm.Services;

namespace UniDocuments.Text.Plagiarism.Matching.Services;

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