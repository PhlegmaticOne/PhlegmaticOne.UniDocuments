using PhlegmaticOne.UniDocuments.Documents.Algorithms.TsSs;
using PhlegmaticOne.UniDocuments.Documents.Core;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;
using PhlegmaticOne.UniDocuments.TextProcessing;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

const string originalText = "Как я встретил вашу маму";
const string comparingText = "Как я встретил вашего папу";

var original = UniDocument.Empty
    .AddFeature<IUniDocumentTextFeature>(new UniDocumentFeatureText(originalText));

var comparing = UniDocument.Empty
    .AddFeature<IUniDocumentTextFeature>(new UniDocumentFeatureText(comparingText));

var algorithm = new PlagiarismAlgorithmTsSs(new TextProcessorML());

var result = algorithm.Perform(comparing, original);

Console.WriteLine($"TS-SS similarity: {result.GetRawValue()}");