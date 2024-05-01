using System.Text.RegularExpressions;
using UniDocuments.Text.Domain.Services.Preprocessing;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading;

public class WordCountApproximatorRegex : IWordsCountApproximator
{
    private const string Space = " ";
    
    private readonly IStopWordsService _stopWordsService;
    private readonly Regex _regex;
    
    public WordCountApproximatorRegex(
        ITextProcessOptionsProvider textProcessOptionsProvider, 
        IStopWordsService stopWordsService)
    {
        _stopWordsService = stopWordsService;
        _regex = new Regex(textProcessOptionsProvider.GetOptions().TokenizeRegex);
    }
    
    public int ApproximateWordsCount(string text)
    {
        var words = _regex
            .Replace(text, Space)
            .Split(Space, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        
        return words.Count(x => !_stopWordsService.IsStopWord(x));
    }
}