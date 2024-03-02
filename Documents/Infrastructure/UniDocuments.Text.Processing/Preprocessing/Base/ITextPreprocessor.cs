using UniDocuments.Text.Processing.Preprocessing.Models;

namespace UniDocuments.Text.Processing.Preprocessing.Base;

public interface ITextPreprocessor
{
    PreprocessorTextOutput ProcessText(PreprocessorTextInput preprocessorTextInput);
}
