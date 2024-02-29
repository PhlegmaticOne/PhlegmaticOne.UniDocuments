using Microsoft.ML.Transforms.Text;
using Microsoft.ML;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.Text.Stemming;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.Text;

public class TextTransformerFactory
{
    private const StopWordsRemovingEstimator.Language Language = StopWordsRemovingEstimator.Language.Russian;
    private const string TextField = nameof(TextInput.Text);
    private const string WordsField = nameof(TextOutput.Words);

    public static PredictionEngine<TextInput, TextOutput> CreateTextTransformer()
    {
        var mlContext = new MLContext();
        var emptySamples = new List<TextInput>();
        var stemmingAction = new StemmingCustomAction();
        var emptyDataView = mlContext.Data.LoadFromEnumerable(emptySamples);
        var transforms = mlContext.Transforms;
        var textTransform = mlContext.Transforms.Text;

        var normalizeTextPipeline = textTransform
            .NormalizeText(TextField, keepPunctuations: false)
            .Append(textTransform.TokenizeIntoWords(WordsField, TextField))
            .Append(textTransform.RemoveDefaultStopWords(WordsField, language: Language))
            .Append(transforms.CustomMapping(stemmingAction.GetMapping(), contractName: WordsField));

        var textTransformer = normalizeTextPipeline.Fit(emptyDataView);

        return mlContext.Model
            .CreatePredictionEngine<TextInput, TextOutput>(textTransformer);
    }
}
