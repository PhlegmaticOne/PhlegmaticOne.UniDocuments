using System.Text;
using UniDocuments.Text.Plagiarism.Winnowing.Algorithm.Algorithm;
using UniDocuments.Text.Processing.Preprocessing;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var originalText = File.ReadAllText(
    @"C:\Users\lolol\Downloads\Plagiarism-Detection-master\Plagiarism-Detection-master\data\orig_taskb.txt");

var plagiatedText = File.ReadAllText(
    @"C:\Users\lolol\Downloads\Plagiarism-Detection-master\Plagiarism-Detection-master\data\g0pA_taskb.txt");

// var original = UniDocument.Empty
//     .AddFeature<IUniDocumentFeatureText>(new UniDocumentFeatureText(originalText));
//
// var comparing = UniDocument.Empty
//     .AddFeature<IUniDocumentFeatureText>(new UniDocumentFeatureText(plagiatedText));

var preprocessor = new TextPreprocessor();
var hashAlgorithm = new FingerprintHashCrc32C();
var algorithm = new TextWinnowing(preprocessor, hashAlgorithm);

var originalWinnowing = algorithm.Winnowing(originalText);
var plagiatedWinnowing = algorithm.Winnowing(plagiatedText);

foreach (var winnowingEntry in plagiatedWinnowing)
{
    if (originalWinnowing.HasFingerprint(winnowingEntry))
    {
        Console.WriteLine($"Plagiated: {winnowingEntry}");
    }
}