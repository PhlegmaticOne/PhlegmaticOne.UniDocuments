using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Doc2Vec.Options;

public class Doc2VecOptionsProvider : INeuralOptionsProvider<Doc2VecOptions>
{
    private readonly ITextProcessOptionsProvider _textProcessOptionsProvider;
    private Doc2VecOptions _options;

    public Doc2VecOptionsProvider(
        IOptionsMonitor<Doc2VecOptions> options, 
        ITextProcessOptionsProvider textProcessOptionsProvider)
    {
        _options = options.CurrentValue;
        _textProcessOptionsProvider = textProcessOptionsProvider;
        options.OnChange(o => _options = o);
    }
    
    public Doc2VecOptions GetOptions()
    {
        _options.TokenizeRegex = _textProcessOptionsProvider.GetOptions().TokenizeRegex;
        return _options;
    }
}