using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading.Options;

public class TextProcessOptionsProvider : ITextProcessOptionsProvider
{
    private TextProcessOptions _options;

    public TextProcessOptionsProvider(IOptionsMonitor<TextProcessOptions> options)
    {
        _options = options.CurrentValue;
        options.OnChange(o => _options = o);
    }
    
    public TextProcessOptions GetOptions()
    {
        return _options;
    }
}