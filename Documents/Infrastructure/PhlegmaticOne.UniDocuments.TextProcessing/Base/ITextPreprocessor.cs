using PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Models;

namespace PhlegmaticOne.UniDocuments.Documents.Algorithms.TextPreprocessing.Base;

public interface ITextPreprocessor
{
    TextOutput ProcessText(TextInput textInput);
}
