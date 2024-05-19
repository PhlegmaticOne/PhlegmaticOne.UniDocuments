using Microsoft.ML.Transforms;
using UniDocuments.Text.Domain.Services.Preprocessing.Preprocessor;
using UniDocuments.Text.Domain.Services.Preprocessing.Stemming;

namespace UniDocuments.Text.Services.Preprocessing.Stemming;

[CustomMappingFactoryAttribute(contractName: nameof(PreprocessorTextOutput.Words))]
public class StemmingCustomAction : CustomMappingFactory<PreprocessorTextOutput, PreprocessorTextOutput>
{
    private const string RemoveCharOne = "\u200B";
    
    private readonly IStemmer _stemmer;
    private const int MinLength = 3;

    public StemmingCustomAction(IStemmer stemmer)
    {
        _stemmer = stemmer;
    }
    
    public override Action<PreprocessorTextOutput, PreprocessorTextOutput> GetMapping()
    {
        return StemWords;
    }

    private void StemWords(PreprocessorTextOutput input, PreprocessorTextOutput output)
    {
        var words = input.Words;
        var result = new List<string>();

        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];

            if (word.Length > MinLength)
            {
                var stemmed = _stemmer.Stem(Preprocess(word));
                result.Add(stemmed);
            }
        }

        output.Words = result.ToArray();
    }

    private static string Preprocess(string input)
    {
        return input.Replace(RemoveCharOne, string.Empty);
    }
}
