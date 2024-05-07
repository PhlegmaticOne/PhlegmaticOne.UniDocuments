using Microsoft.ML;
using UniDocuments.Text.Domain.Services.Preprocessing.Preprocessor;
using UniDocuments.Text.Domain.Services.Preprocessing.Stemming;

namespace UniDocuments.Text.Services.Preprocessing.Preprocessor;

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