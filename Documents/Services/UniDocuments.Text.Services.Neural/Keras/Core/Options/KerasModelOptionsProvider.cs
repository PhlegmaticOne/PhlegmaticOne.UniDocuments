using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Keras.Core.Options;

public class KerasModelOptionsProvider<T> : INeuralOptionsProvider<T> where T : KerasModelOptions, new()
{
    private readonly IOptions<T> _options;
    private readonly ITextProcessOptionsProvider _optionsProvider;

    public KerasModelOptionsProvider(IOptions<T> options, ITextProcessOptionsProvider optionsProvider)
    {
        _options = options;
        _optionsProvider = optionsProvider;
    }
    
    public T GetOptions()
    {
        var options = _options.Value;
        options.TokenizeRegex = _optionsProvider.GetOptions().TokenizeRegex;
        return options;
    }
}