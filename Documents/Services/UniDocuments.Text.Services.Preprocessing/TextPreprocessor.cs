using Microsoft.ML;
using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.Text.Services.Preprocessing;

public class TextPreprocessor : ITextPreprocessor
{
    private readonly PredictionEngine<PreprocessorTextInput, PreprocessorTextOutput> _textTransformer;

    public TextPreprocessor(IStemmer stemmer)
    {
        _textTransformer = TextPreprocessorEngineFactory.CreateTransformer(stemmer);
    }

    public PreprocessorTextOutput Preprocess(PreprocessorTextInput preprocessorTextInput)
    {
        return _textTransformer.Predict(preprocessorTextInput);
    }
}