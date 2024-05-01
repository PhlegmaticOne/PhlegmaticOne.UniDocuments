namespace UniDocuments.Text.Services.Neural.Common;

public class InferOptionsRaw : IInferOptions
{
    public InferOptionsRaw(string tokenizeRegex)
    {
        TokenizeRegex = tokenizeRegex;
    }

    public string TokenizeRegex { get; }
}