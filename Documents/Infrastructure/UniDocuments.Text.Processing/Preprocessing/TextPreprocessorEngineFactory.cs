using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using UniDocuments.Text.Processing.Preprocessing.Models;
using UniDocuments.Text.Processing.Stemming;

namespace UniDocuments.Text.Processing.Preprocessing;

public static class TextPreprocessorEngineFactory
{
    private const StopWordsRemovingEstimator.Language Language = StopWordsRemovingEstimator.Language.Russian;
    private const string TextField = nameof(PreprocessorTextInput.Text);
    private const string WordsField = nameof(PreprocessorTextOutput.Words);

    public static PredictionEngine<PreprocessorTextInput, PreprocessorTextOutput> CreateTransformer()
    {
        var mlContext = new MLContext();
        var stemmingAction = new StemmingCustomAction();
        var emptyDataView = mlContext.Data.LoadFromEnumerable(new List<PreprocessorTextInput>());
        var transforms = mlContext.Transforms;
        var textTransform = mlContext.Transforms.Text;

        var normalizeTextPipeline = textTransform
            .NormalizeText(TextField, keepPunctuations: false, keepNumbers: false)
            .Append(textTransform.TokenizeIntoWords(WordsField, TextField))
            .Append(textTransform.RemoveDefaultStopWords(WordsField, language: Language))
            .Append(transforms.CustomMapping(stemmingAction.GetMapping(), contractName: WordsField));

        var textTransformer = normalizeTextPipeline.Fit(emptyDataView);

        return mlContext.Model.CreatePredictionEngine<PreprocessorTextInput, PreprocessorTextOutput>(textTransformer);
    }
}
