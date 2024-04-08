using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.StreamReading;

public class StreamContentReaderWordDocument : IStreamContentReader
{
    public Task<StreamContentReadResult> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        var result = new StreamContentReadResult();
        
        using (var wordDocument = WordprocessingDocument.Open(stream, false))
        {
            var body = wordDocument.MainDocumentPart.Document.Body;

            foreach (OpenXmlElement xmlElement in body.ChildElements)
            {
                var text = xmlElement.InnerText;
                
                if (string.IsNullOrWhiteSpace(text) == false)
                {
                    result.AddParagraph(text);
                }
            }
        }
        
        return Task.FromResult(result);
    }
}