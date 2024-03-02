using Microsoft.ML;
using UniDocuments.Text.Processing.Preprocessing.Base;
using UniDocuments.Text.Processing.Preprocessing.Models;

namespace UniDocuments.Text.Processing.Preprocessing;

public class TextPreprocessor : ITextPreprocessor
{
    private readonly PredictionEngine<PreprocessorTextInput, PreprocessorTextOutput> _textTransformer;

    public TextPreprocessor()
    {
        _textTransformer = TextPreprocessorEngineFactory.CreateTransformer();
    }

    public PreprocessorTextOutput ProcessText(PreprocessorTextInput preprocessorTextInput)
    {
        return _textTransformer.Predict(preprocessorTextInput);
    }
}