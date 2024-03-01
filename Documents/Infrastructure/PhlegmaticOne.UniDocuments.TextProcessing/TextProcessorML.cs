using Microsoft.ML;
using PhlegmaticOne.UniDocuments.TextProcessing.Base;
using PhlegmaticOne.UniDocuments.TextProcessing.Models;

namespace PhlegmaticOne.UniDocuments.TextProcessing;

public class TextProcessorML : ITextPreprocessor
{
    private readonly PredictionEngine<TextInput, TextOutput> _textTransformer;

    public TextProcessorML()
    {
        _textTransformer = TextTransformerFactory.CreateTextTransformer();
    }

    public TextOutput ProcessText(TextInput textInput)
    {
        return _textTransformer.Predict(textInput);
    }
}