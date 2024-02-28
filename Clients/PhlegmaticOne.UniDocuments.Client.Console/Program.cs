using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

//const string connectionString = "Server=localhost\\SQLEXPRESS;Database=uni_documents_db;Trusted_Connection=True;Encrypt=False;";

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var text = "ML.NET's NormalizeText API " +
           "changes the case of the TEXT and removes/keeps diâcrîtîcs, " +
           "punctuations, and/or numbers (123).";

var mlContext = new MLContext();
var emptySamples = new List<TextInput>();
var emptyDataView = mlContext.Data.LoadFromEnumerable(emptySamples);

var textProcessor = mlContext.Transforms.Text;
var normTextPipeline = textProcessor
    .NormalizeText("Text", keepPunctuations: false)
    .Append(textProcessor.TokenizeIntoWords("Words", "Text"))
    .Append(textProcessor.RemoveDefaultStopWords("Words", language: StopWordsRemovingEstimator.Language.English));

// Fit to data.
var textTransformer = normTextPipeline.Fit(emptyDataView);
var predictionEngine = mlContext.Model
    .CreatePredictionEngine<TextInput, TextOutputProceed>(textTransformer);

var data = new TextInput()
{
    Text = text
};

var prediction = predictionEngine.Predict(data);

// Print the normalized text.
Console.WriteLine($"\nWords: {string.Join(",", prediction.Words)}");


class TextInput
{
    public string Text { get; set; }
}

class TextOutputProceed : TextInput
{
    public string[] Words { get; set; }
}