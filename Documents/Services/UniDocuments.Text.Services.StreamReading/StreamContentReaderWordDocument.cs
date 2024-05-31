using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Extensions;
using UniDocuments.Text.Domain.Services.Preprocessing.Utilities;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading;

public class StreamContentReaderWordDocument : IStreamContentReader
{
    private const string Paragraph = "Paragraph";
    
    private readonly ITextProcessOptionsProvider _textProcessOptionsProvider;
    private readonly IWordsCountApproximator _wordsCountApproximator;

    public StreamContentReaderWordDocument(
        ITextProcessOptionsProvider textProcessOptionsProvider,
        IWordsCountApproximator wordsCountApproximator)
    {
        _textProcessOptionsProvider = textProcessOptionsProvider;
        _wordsCountApproximator = wordsCountApproximator;
    }

    public UniDocumentContent Read(Stream stream)
    {
        return ReadAsyncPrivate(stream);
    }

    public Task<UniDocumentContent> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        return Task.Run(() => ReadAsyncPrivate(stream), cancellationToken);
    }

    private UniDocumentContent ReadAsyncPrivate(Stream stream)
    {
        var paragraphs = new List<string>();
        var options = _textProcessOptionsProvider.GetOptions();
        var breakTexts = options.BreakTexts.ToHashSet(new StringComparerOrdinalIgnoreCase());
        stream.SeekToZero();
        
        using var wordDocument = WordprocessingDocument.Open(stream, false);
        var body = wordDocument.MainDocumentPart.Document.Body;

        foreach (OpenXmlElement xmlElement in body.ChildElements)
        {
            if (!TryGetParagraphText(xmlElement, out var text))
            {
                continue;
            }

            if (breakTexts.Contains(text!))
            {
                break;
            }

            var wordsCount = _wordsCountApproximator.ApproximateWordsCount(text!);
                
            if (wordsCount >= options.MinWordsCount)
            {
                paragraphs.Add(text!);
            }
        }

        return new UniDocumentContent(paragraphs);
    }

    private static bool TryGetParagraphText(OpenXmlElement xmlElement, out string? text)
    {
        text = null;
        var xmlString = xmlElement.ToString();
                
        if (string.IsNullOrEmpty(xmlString) || !xmlString.Contains(Paragraph))
        {
            return false;
        }
                
        var innerText = xmlElement.InnerText;

        if (string.IsNullOrWhiteSpace(innerText))
        {
            return false;
        }

        text = innerText.Trim();
        return true;
    }
}