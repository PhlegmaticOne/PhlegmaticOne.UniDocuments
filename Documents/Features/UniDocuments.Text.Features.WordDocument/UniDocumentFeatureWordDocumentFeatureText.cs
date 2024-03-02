using System.Text;
using Aspose.Words;
using UniDocuments.Text.Core.Features;
using UniDocuments.Text.Core.Features.Content;

namespace UniDocuments.Text.Features.WordDocument;

public class UniDocumentFeatureWordDocumentFeatureText : IUniDocumentFeatureText
{
    private readonly Document? _wordDocument;

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Text;

    public UniDocumentFeatureWordDocumentFeatureText(Stream stream)
    {
        _wordDocument = ReadFromStream(stream);
    }

    public string GetText()
    {
        if (_wordDocument == null)
        {
            return string.Empty;
        }

        var isContentStarted = false;
        var isContentEnd = false;
        var sb = new StringBuilder();

        foreach (var node in _wordDocument.GetChildNodes(NodeType.Paragraph, true))
        {
            var paragraph = (Paragraph)node;
            var text = paragraph.ToString(SaveFormat.Text).ToLower();

            if (!isContentStarted)
            {
                if (text.Contains("гомель ", StringComparison.InvariantCultureIgnoreCase))
                {
                    isContentStarted = true;
                }

                continue;
            }

            if (!isContentEnd)
            {
                if (text.Contains("приложение а", StringComparison.InvariantCultureIgnoreCase))
                {
                    isContentEnd = true;
                }
                else
                {
                    sb.AppendLine(text.Trim());
                }
            }
        }

        return sb.ToString();
    }

    private static Document ReadFromStream(Stream stream)
    {
        var document = new Document(stream);
        stream.Dispose();
        return document;
    }
}
