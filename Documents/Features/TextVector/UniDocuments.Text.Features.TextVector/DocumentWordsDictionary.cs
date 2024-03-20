namespace UniDocuments.Text.Features.TextVector;

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