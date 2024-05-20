using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Options;

public class KerasModelOptionsProvider : INeuralOptionsProvider<KerasModelOptions>
{
    private readonly ITextProcessOptionsProvider _optionsProvider;
    
    private KerasModelOptions _options;

    public KerasModelOptionsProvider(
        IOptionsMonitor<KerasModelOptions> options,
        ITextProcessOptionsProvider optionsProvider)
    {
        _options = options.CurrentValue;
        _optionsProvider = optionsProvider;
        options.OnChange(o => _options = o);
    }
    
    public KerasModelOptions GetOptions()
    {
        _options.TokenizeRegex = _optionsProvider.GetOptions().TokenizeRegex;
        return _options;
    }
}