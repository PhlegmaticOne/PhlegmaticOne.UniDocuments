using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Options;

public class KerasModelOptionsProvider : INeuralOptionsProvider<KerasModelOptions>
{
    private readonly IOptions<KerasModelOptions> _options;
    private readonly ITextProcessOptionsProvider _optionsProvider;

    public KerasModelOptionsProvider(IOptions<KerasModelOptions> options, ITextProcessOptionsProvider optionsProvider)
    {
        _options = options;
        _optionsProvider = optionsProvider;
    }
    
    public KerasModelOptions GetOptions()
    {
        var options = _options.Value;
        options.TokenizeRegex = _optionsProvider.GetOptions().TokenizeRegex;
        return options;
    }
}