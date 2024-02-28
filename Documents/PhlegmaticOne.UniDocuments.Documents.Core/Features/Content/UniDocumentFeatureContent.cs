using Aspose.Words;
using System.Text;

namespace PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;

public class UniDocumentFeatureContent : IUniDocumentFeature
{
    private readonly Stream _contentStream;

    private Document? _wordDocument;

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Content;

    public UniDocumentFeatureContent(Stream stream)
    {
        _contentStream = stream;
    }

    public string GetDocumentContentAsString()
    {
        if (_wordDocument == null)
        {
            return string.Empty;
        }

        var isContentStarted = false;
        var isContentEnd = false;
        var sb = new StringBuilder();

        foreach (Paragraph paragraph in _wordDocument.GetChildNodes(NodeType.Paragraph, true))
        {
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
