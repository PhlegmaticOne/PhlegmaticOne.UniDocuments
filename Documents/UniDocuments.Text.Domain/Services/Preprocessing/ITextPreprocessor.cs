namespace UniDocuments.Text.Domain.Services.Preprocessing;

public interface ITextPreprocessor
{
    PreprocessorTextOutput Preprocess(PreprocessorTextInput preprocessorTextInput);
}
