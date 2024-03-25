using System.Text;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using UniDocuments.Text.Domain.Services.StreamReading;

namespace UniDocuments.Text.Services.StreamReading;

public class StreamContentReaderWordDocument : IStreamContentReader
{
    private const string WordmlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

    public Task<string> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        var textBuilder = new StringBuilder();
        
        using (var wordDocument = WordprocessingDocument.Open(stream, false))
        {
            var nameTable = new NameTable();
            var namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("w", WordmlNamespace);

            var document = new XmlDocument(nameTable);
            document.Load(wordDocument.MainDocumentPart.GetStream());
            
            var paragraphNodes = document.SelectNodes("//w:p", namespaceManager);
            
            foreach (XmlNode paragraphNode in paragraphNodes!)
            {
                var textNodes = paragraphNode.SelectNodes(".//w:t", namespaceManager);
                
                foreach (XmlNode textNode in textNodes!)
                {
                    textBuilder.Append(textNode.InnerText);
                }
            }
        }
        
        var result = textBuilder.ToString();
        return Task.FromResult(result);
    }
}