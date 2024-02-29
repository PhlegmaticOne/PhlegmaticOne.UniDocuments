using PhlegmaticOne.UniDocuments.Documents.Algorithms.Cosine;
using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing;
using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var originalText = "немного очень интересного текста";
var comparingText = "очень немного текста, но интересного";

var original = UniDocument.Empty
    .AddFeature<IUniDocumentTextFeature>(new UniDocumentFeatureText(originalText));

var comparing = UniDocument.Empty
    .AddFeature<IUniDocumentTextFeature>(new UniDocumentFeatureText(comparingText));

var algorithm = new PlagiarismAlgorithmCosineSimilarity(new TextProcessorML());

var result = algorithm.Perform(comparing, original);

Console.WriteLine($"Cosine similarity: {result.GetRawValue()}");