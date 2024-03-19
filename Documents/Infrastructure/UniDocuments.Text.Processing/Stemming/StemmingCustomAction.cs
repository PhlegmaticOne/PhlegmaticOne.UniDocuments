using Microsoft.ML.Transforms;
using UniDocuments.Text.Processing.Preprocessing.Models;

namespace UniDocuments.Text.Processing.Stemming;

[CustomMappingFactoryAttribute(contractName: nameof(PreprocessorTextOutput.Words))]
public class StemmingCustomAction : CustomMappingFactory<PreprocessorTextOutput, PreprocessorTextOutput>
{
    private const int MinLength = 2;
    
    public override Action<PreprocessorTextOutput, PreprocessorTextOutput> GetMapping()
    {
        return StemWords;
    }

    private static void StemWords(PreprocessorTextOutput input, PreprocessorTextOutput output)
    {
        var words = input.Words;
        var result = new List<string>();

        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];

            if (word.Length > MinLength)
            {
                var stemmed = Stemmer.Stem(word);
                result.Add(stemmed);
            }
        }

        output.Words = result.ToArray();
    }
}
