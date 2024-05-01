using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using UniDocuments.App.Client.Console;

var path = @"C:\Users\lolol\Downloads\train.json\train.json";
var resultPath = @"C:\Users\lolol\Downloads\TestDocuments\large.docx";
var file = File.ReadAllText(path);
var json = JsonConvert.DeserializeObject<Test[]>(file);

using var doc = WordprocessingDocument.Create(resultPath, WordprocessingDocumentType.Document);
var mainPart = doc.AddMainDocumentPart();
mainPart.Document = new Document();
var body = mainPart.Document.AppendChild(new Body());

foreach (var test in json!)
{
    body.Append(CreateParagraph(test.text));
}

doc.Save();

return;

static Paragraph CreateParagraph(string text)
{
    Paragraph paragraph = new Paragraph();
    Run run = new Run(new Text(text));
    paragraph.Append(run);
    return paragraph;
}

namespace UniDocuments.App.Client.Console
{
    class Test
    {
        public string text { get; set; }
    }
}