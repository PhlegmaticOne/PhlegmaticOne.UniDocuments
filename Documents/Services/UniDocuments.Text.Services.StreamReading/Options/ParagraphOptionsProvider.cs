using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading.Options;

public class ParagraphOptionsProvider : IParagraphOptionsProvider
{
    private readonly IOptions<ParagraphOptions> _options;

    public ParagraphOptionsProvider(IOptions<ParagraphOptions> options)
    {
        _options = options;
    }
    
    public ParagraphOptions GetOptions()
    {
        return _options.Value;
    }
}