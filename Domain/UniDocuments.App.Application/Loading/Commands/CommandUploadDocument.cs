using System.Text;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;

namespace UniDocuments.App.Application.Loading.Commands;

public class CommandUploadDocument : IdentityOperationResultCommand
{
    public Stream DocumentStream { get; }

    public CommandUploadDocument(Guid profileId, Stream documentStream) : base(profileId)
    {
        DocumentStream = documentStream;
    }
}

public class CommandUploadDocumentHandler : IOperationResultCommandHandler<CommandUploadDocument>
{
    public Task<OperationResult> Handle(CommandUploadDocument request, CancellationToken cancellationToken)
    {
        return Task.FromResult(OperationResult.Success);
    }
    
    private static string TextFromWord(Stream formFile)
    {
        const string wordmlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        var textBuilder = new StringBuilder();
        
        using (var wdDoc = WordprocessingDocument.Open(formFile, false))
        {
            var nt = new NameTable();
            var nsManager = new XmlNamespaceManager(nt);
            nsManager.AddNamespace("w", wordmlNamespace);

            var document = new XmlDocument(nt);
            document.Load(wdDoc.MainDocumentPart.GetStream());
            
            var paragraphNodes = document.SelectNodes("//w:p", nsManager);
            
            foreach (XmlNode paragraphNode in paragraphNodes)
            {
                var textNodes = paragraphNode.SelectNodes(".//w:t", nsManager);
                
                foreach (XmlNode textNode in textNodes)
                {
                    textBuilder.Append(textNode.InnerText);
                }
            }
        }
        
        return textBuilder.ToString();
    }
}