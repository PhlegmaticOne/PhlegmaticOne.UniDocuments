namespace UniDocuments.Text.Core.Features.Content;

public class UniDocumentFeatureText : IUniDocumentFeatureText
{
    private readonly string _text;

    public UniDocumentFeatureText(string text)
    {
        _text = text;
    }

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Text;

    public string GetText()
    {
        return _text;   
    }
}
