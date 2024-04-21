using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading.Options;

public class TextProcessOptionsProvider : ITextProcessOptionsProvider
{
    private readonly IOptions<TextProcessOptions> _options;

    public TextProcessOptionsProvider(IOptions<TextProcessOptions> options)
    {
        _options = options;
    }
    
    public TextProcessOptions GetOptions()
    {
        return _options.Value;
    }
}