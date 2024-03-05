using UniDocuments.Text.Processing.Preprocessing.Models;

namespace UniDocuments.Text.Processing.Preprocessing.Base;

public interface ITextPreprocessor
{
    PreprocessorTextOutput Preprocess(PreprocessorTextInput preprocessorTextInput);
}
