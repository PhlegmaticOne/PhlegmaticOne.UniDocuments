using Microsoft.ML.Transforms;
using PhlegmaticOne.UniDocuments.TextProcessing.Models;

namespace PhlegmaticOne.UniDocuments.TextProcessing.Stemming;

[CustomMappingFactoryAttribute(contractName: "Words")]
public class StemmingCustomAction : CustomMappingFactory<TextOutput, TextOutput>
{
    public override Action<TextOutput, TextOutput> GetMapping()
    {
        return StemWords;
    }

    private static void StemWords(TextOutput input, TextOutput output)
    {
        var words = input.Words;

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = Stemmer.Stem(words[i]);
        }

        output.Words = words;
    }
}
