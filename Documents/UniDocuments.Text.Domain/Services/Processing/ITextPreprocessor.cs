namespace UniDocuments.Text.Domain.Services.Processing;

public interface ITextPreprocessor
{
    PreprocessorTextOutput Preprocess(PreprocessorTextInput preprocessorTextInput);
}
