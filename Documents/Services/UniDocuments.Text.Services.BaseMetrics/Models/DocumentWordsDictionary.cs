namespace UniDocuments.Text.Services.BaseMetrics.Models;

internal class DocumentWordsDictionary
{
    private readonly Dictionary<string, int> _dictionary;
    
    public DocumentWordsDictionary(IEnumerable<string> words)
    {
        _dictionary = CreateWordsDictionary(words);
    }

    public int GetWordEntriesCount(string word)
    {
        return _dictionary.GetValueOrDefault(word, 0);
    }

    public HashSet<string> Merge(DocumentWordsDictionary wordsDictionary)
    {
        var result = new HashSet<string>();

        foreach (var word in _dictionary)
        {
            result.Add(word.Key);
        }

        foreach (var word in wordsDictionary._dictionary)
        {
            result.Add(word.Key);
        }

        return result;
    }
    
    private static Dictionary<string, int> CreateWordsDictionary(IEnumerable<string> words)
    {
        var result = new Dictionary<string, int>();

        foreach (var word in words)
        {
            if (!result.TryAdd(word, 1))
            {
                result[word]++;
            }
        }

        return result;
    }
}


// public Task<IUniDocumentFeature> CreateFeature(UniDocumentEntry documentEntry, CancellationToken cancellationToken)
// {
//     var originalWords = CreateDocumentDictionary(documentEntry.Original);
//     var comparingWords = CreateDocumentDictionary(documentEntry.Comparing);
//         
//     var allKeys = originalWords.Merge(comparingWords);
//         
//     var originalVector = UniVector<int>.FromEnumerating(allKeys, w => originalWords.GetWordEntriesCount(w));
//     var comparingVector = UniVector<int>.FromEnumerating(allKeys, w => comparingWords.GetWordEntriesCount(w));
//
//     IUniDocumentFeature feature = new UniDocumentFeatureTextVector(originalVector, comparingVector);
//     return Task.FromResult(feature);
// }
//
// private DocumentWordsDictionary CreateDocumentDictionary(UniDocument document)
// {
//     var textFeature = document.GetFeature<UniDocumentFeatureText>();
//         
//     var proceedText = _textPreprocessor.Preprocess(new PreprocessorTextInput
//     {
//         Text = textFeature.Content.ToRawText()
//     });
//         
//     return new DocumentWordsDictionary(proceedText.Words);
// }