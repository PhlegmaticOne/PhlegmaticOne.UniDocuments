using PhlegmaticOne.UniDocuments.TextProcessing.Models;

namespace PhlegmaticOne.UniDocuments.TextProcessing.Base;

public interface ITextPreprocessor
{
    TextOutput ProcessText(TextInput textInput);
}
