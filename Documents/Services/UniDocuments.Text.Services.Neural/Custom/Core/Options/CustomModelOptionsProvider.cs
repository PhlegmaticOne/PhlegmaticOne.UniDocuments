using Microsoft.Extensions.Options;
using UniDocuments.Text.Domain.Services.Neural.Options;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.Neural.Custom.Core.Options;

public class CustomModelOptionsProvider<T> : INeuralOptionsProvider<T> where T : CustomModelOptions, new()
{
    private readonly IOptions<T> _options;
    private readonly ITextProcessOptionsProvider _optionsProvider;

    public CustomModelOptionsProvider(IOptions<T> options, ITextProcessOptionsProvider optionsProvider)
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