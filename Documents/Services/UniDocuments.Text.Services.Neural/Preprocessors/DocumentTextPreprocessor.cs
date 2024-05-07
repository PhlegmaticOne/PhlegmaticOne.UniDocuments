using PhlegmaticOne.PythonTasks;
using UniDocuments.Text.Domain.Services.Neural;
using UniDocuments.Text.Domain.Services.StreamReading.Options;
using UniDocuments.Text.Services.Neural.Preprocessors.Tasks;

namespace UniDocuments.Text.Services.Neural.Preprocessors;

public class DocumentTextPreprocessor : IDocumentTextPreprocessor
{
    private readonly ITextProcessOptionsProvider _textProcessOptionsProvider;

    public DocumentTextPreprocessor(ITextProcessOptionsProvider textProcessOptionsProvider)
    {
        _textProcessOptionsProvider = textProcessOptionsProvider;
    }
    
    public async Task<string> Preprocess(string text, CancellationToken cancellationToken)
    {
        var options = _textProcessOptionsProvider.GetOptions();
        var input = new PreprocessTextInput(text, options.TokenizeRegex);
        var result = await new PythonTaskPreprocessText(input);
        return result!;
    }
}