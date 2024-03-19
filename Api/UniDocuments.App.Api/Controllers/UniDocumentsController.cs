using System.Text;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private readonly ILogger<UniDocumentsController> _logger;

    public UniDocumentsController(ILogger<UniDocumentsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile formFile)
    {
        var text = TextFromWord(formFile);
        await System.IO.File.WriteAllTextAsync(@"C:\Users\lolol\Downloads\test.txt", text);
        _logger.LogInformation("UploadFile" + formFile.FileName);
        IActionResult r = Ok(formFile.FileName);
        return r;
    }

    private static string TextFromWord(IFormFile formFile)
    {
        const string wordmlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        var textBuilder = new StringBuilder();
        
        using (var wdDoc = WordprocessingDocument.Open(formFile.OpenReadStream(), false))
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