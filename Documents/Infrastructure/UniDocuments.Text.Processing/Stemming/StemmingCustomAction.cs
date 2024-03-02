using Microsoft.ML.Transforms;
using UniDocuments.Text.Processing.Preprocessing.Models;

namespace UniDocuments.Text.Processing.Stemming;

[CustomMappingFactoryAttribute(contractName: nameof(PreprocessorTextOutput.Words))]
public class StemmingCustomAction : CustomMappingFactory<PreprocessorTextOutput, PreprocessorTextOutput>
{
    public override Action<PreprocessorTextOutput, PreprocessorTextOutput> GetMapping()
    {
        return StemWords;
    }

    private static void StemWords(PreprocessorTextOutput input, PreprocessorTextOutput output)
    {
        var words = input.Words;

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = Stemmer.Stem(words[i]);
        }

        output.Words = words;
    }
}
