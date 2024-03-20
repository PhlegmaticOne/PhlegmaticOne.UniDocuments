using Microsoft.ML;
using UniDocuments.Text.Domain.Services.Processing;

namespace UniDocuments.Text.Processing.Preprocessing;

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