﻿using System.Text;
using Aspose.Words;
using PhlegmaticOne.UniDocuments.Documents.Core.Features;
using PhlegmaticOne.UniDocuments.Documents.Core.Features.Content;

namespace PhlegmaticOne.UniDocuments.Documents.Features.WordDocument;

public class UniDocumentFeatureWordDocumentText : IUniDocumentTextFeature
{
    private readonly Stream _contentStream;

    private Document? _wordDocument;

    public UniDocumentFeatureFlag FeatureFlag => UniDocumentFeatureFlag.Text;

    public UniDocumentFeatureWordDocumentText(Stream stream)
    {
        _contentStream = stream;
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
