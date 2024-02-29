using Microsoft.ML;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.Text;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Base;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing;

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