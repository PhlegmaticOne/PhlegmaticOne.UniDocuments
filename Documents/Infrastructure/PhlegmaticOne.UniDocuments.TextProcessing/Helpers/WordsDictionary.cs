namespace PhlegmaticOne.UniDocuments.TextProcessing.Helpers;

public static class WordsDictionary
{
    public static Dictionary<string, int> CreateWordsDictionary(string[] words)
    {
        var result = new Dictionary<string, int>();

        foreach (var word in words)
        {
            if (result.ContainsKey(word))
            {
                result[word]++;
            }
            else
            {
                result.Add(word, 1);
            }
        }

        return result;
    }

    public static HashSet<string> GetAllKeys(Dictionary<string, int> a, Dictionary<string, int> b)
    {
        var result = new HashSet<string>();

        foreach (var word in a)
        {
            result.Add(word.Key);
        }

        foreach (var word in b)
        {
            result.Add(word.Key);
        }

        return result;
    }
}
