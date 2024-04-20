using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using UniDocuments.Text.Domain;
using UniDocuments.Text.Domain.Services.StreamReading;
using UniDocuments.Text.Domain.Services.StreamReading.Options;

namespace UniDocuments.Text.Services.StreamReading;

public class StreamContentReaderWordDocument : IStreamContentReader
{
    private readonly IParagraphOptionsProvider _paragraphOptionsProvider;

    public StreamContentReaderWordDocument(IParagraphOptionsProvider paragraphOptionsProvider)
    {
        _paragraphOptionsProvider = paragraphOptionsProvider;
    }
    
    public Task<UniDocumentContent> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        var result = new UniDocumentContent();
        var options = _paragraphOptionsProvider.GetOptions();
        
        using (var wordDocument = WordprocessingDocument.Open(stream, false))
        {
            var body = wordDocument.MainDocumentPart.Document.Body;

            foreach (OpenXmlElement xmlElement in body.ChildElements)
            {
                var text = xmlElement.InnerText;
                var wordsCount = GetWordsCountApproximate(text);
                
                if (string.IsNullOrWhiteSpace(text) == false && wordsCount >= options.MinWordsCount)
                {
                    result.AddParagraph(text);
                }
            }
        }
        
        return Task.FromResult(result);
    }

    private static int GetWordsCountApproximate(string content)
    {
        return content.Count(x => x == ' ') + 1;
    }
}