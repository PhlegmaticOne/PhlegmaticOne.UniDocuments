﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using UniDocuments.Text.Domain;
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
    
    public Task<UniDocumentContent> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        return Task.Run(() => ReadAsyncPrivate(stream), cancellationToken);
    }

    private UniDocumentContent ReadAsyncPrivate(Stream stream)
    {
        var result = new UniDocumentContent();
        var options = _textProcessOptionsProvider.GetOptions();

        using var wordDocument = WordprocessingDocument.Open(stream, false);
        var body = wordDocument.MainDocumentPart.Document.Body;

        foreach (OpenXmlElement xmlElement in body.ChildElements)
        {
            if (!TryGetParagraphText(xmlElement, out var text))
            {
                continue;
            }

            var wordsCount = _wordsCountApproximator.ApproximateWordsCount(text!);
                
            if (wordsCount >= options.MinWordsCount)
            {
                result.AddParagraph(text!);
            }
        }

        return result;
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

        text = innerText;
        return true;
    }
}