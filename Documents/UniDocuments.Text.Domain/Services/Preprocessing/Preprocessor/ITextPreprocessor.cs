namespace UniDocuments.Text.Domain.Services.Preprocessing.Preprocessor;

public interface ITextPreprocessor
{
    PreprocessorTextOutput Preprocess(PreprocessorTextInput preprocessorTextInput);
}
