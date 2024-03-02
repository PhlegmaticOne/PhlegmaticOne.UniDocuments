using System.Text;
using UniDocuments.Text.Core;
using UniDocuments.Text.Core.Features.Content;
using UniDocuments.Text.Plagiarism.Matching.Algorithm;
using UniDocuments.Text.Processing.StopWords;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var originalText = File.ReadAllText(
    @"C:\Users\lolol\Downloads\Plagiarism-Detection-master\Plagiarism-Detection-master\data\orig_taskb.txt");

var plagiatedText = File.ReadAllText(
    @"C:\Users\lolol\Downloads\Plagiarism-Detection-master\Plagiarism-Detection-master\data\g0pA_taskb.txt");

var original = UniDocument.Empty
    .AddFeature<IUniDocumentFeatureText>(new UniDocumentFeatureText(originalText));

var comparing = UniDocument.Empty
    .AddFeature<IUniDocumentFeatureText>(new UniDocumentFeatureText(plagiatedText));

var stopWordsLoader = new StopWordsLoaderFile();
var stopWordsService = new StopWordsService(stopWordsLoader);
var tokenSource = new CancellationTokenSource();
await stopWordsService.InitializeAsync(tokenSource.Token);

var algorithm = new PlagiarismAlgorithmMatching(stopWordsService);
var result = algorithm.PerformExact(original, comparing);

foreach (var sequence in result.Matches)
{
    var textOriginal = originalText.Substring(sequence.SourceFragmentStartIndex, sequence.SourceFragmentLength);
    var textComparing = plagiatedText.Substring(sequence.MatchedFragmentStartIndex, sequence.MatchedFragmentLength);
    Console.WriteLine(textOriginal);
    Console.WriteLine(textComparing);
}