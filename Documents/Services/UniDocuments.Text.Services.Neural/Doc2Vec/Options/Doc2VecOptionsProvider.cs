using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Options;

public class Doc2VecOptionsProvider : INeuralOptionsProvider<Doc2VecOptions>
{
    private readonly IOptionsMonitor<Doc2VecOptions> _optionsSnapshot;
    private readonly ITextProcessOptionsProvider _textProcessOptionsProvider;

    public Doc2VecOptionsProvider(
        IOptionsMonitor<Doc2VecOptions> optionsSnapshot, 
        ITextProcessOptionsProvider textProcessOptionsProvider)
    {
        _optionsSnapshot = optionsSnapshot;
        _textProcessOptionsProvider = textProcessOptionsProvider;
    }
    
    public Doc2VecOptions GetOptions()
    {
        var options = _optionsSnapshot.CurrentValue;
        options.TokenizeRegex = _textProcessOptionsProvider.GetOptions().TokenizeRegex;
        return options;
    }
}