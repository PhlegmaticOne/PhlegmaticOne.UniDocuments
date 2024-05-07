using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using UniDocuments.Text.Domain.Extensions;
using UniDocuments.Text.Domain.Services.Reports;
using UniDocuments.Text.Domain.Services.Reports.Models;

namespace UniDocuments.Text.Services.Reporting;

public class PlagiarismReportCreatorWord : IPlagiarismReportCreator
{
    private const string ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    private const string NamePattern = "report_{0}.docx";
    
    private static RunProperties Title => new()
    {
        FontSize = new FontSize { Val = "46" },
        Bold = new Bold()
    };

    private static RunProperties SourceHeader => new()
    {
        FontSize = new FontSize { Val = "32" },
        Bold = new Bold()
    };
    
    private static RunProperties SuspiciousHeader => new()
    {
        FontSize = new FontSize { Val = "26" },
        Bold = new Bold()
    };
    
    private static RunProperties SuspiciousHeaderData => new()
    {
        FontSize = new FontSize { Val = "24" },
        Bold = new Bold()
    };
    
    private static RunProperties Source => new()
    {
        FontSize = new FontSize { Val = "24" },
    };
    
    private static RunProperties Suspicious => new()
    {
        FontSize = new FontSize { Val = "20" },
    };
    
    public Task<PlagiarismReport> BuildReportAsync(
        PlagiarismReportData reportData, CancellationToken cancellationToken)
    {
        return Task.Run(() => BuildReport(reportData), cancellationToken);
    }

    private static PlagiarismReport BuildReport(PlagiarismReportData reportData)
    {
        var fileName = GetFileName(reportData.DocumentName);

        using var stream = new MemoryStream();
        
        using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document, false))
        {
            var body = CreateDocumentBody(document);
            AddParagraphsComparing(reportData, body);
            AddDocumentsComparing(reportData, body);
            document.Clone(stream);
        }

        File.Delete(fileName);
        stream.SeekToZero();
        return new PlagiarismReport(stream.ToArray(), ContentType, fileName);
    }

    private static void AddParagraphsComparing(PlagiarismReportData reportData, Body body)
    {
        AddTitle("Сравнение параграфов", body);

        if (reportData.ParagraphsData.Count == 0)
        {
            AddTitle("Не найдено слишком подозрительных параграфов", body);
            AddEmptyLine(body);
            return;
        }
            
        for (var i = 0; i < reportData.ParagraphsData.Count; i++)
        {
            var paragraphsData = reportData.ParagraphsData[i];

            if (paragraphsData.ParagraphsData.Count == 0)
            {
                continue;
            }
                
            AddSourceParagraph(paragraphsData, i, body);
            AddSuspiciousParagraphs(paragraphsData, body);
            AddEmptyLine(body);
        }
        
        AddEmptyLine(body);
    }

    private static void AddDocumentsComparing(PlagiarismReportData reportData, Body body)
    {
        AddTitle("Сравнение документов", body);
        
        if (reportData.DocumentData.Count == 0)
        {
            AddTitle("Не найдено слишком подозрительных документов", body);
            AddEmptyLine(body);
            return;
        }

        for (var i = 0; i < reportData.DocumentData.Count; i++)
        {
            var documentData = reportData.DocumentData[i];
            AddDocumentData(i, documentData, body);
        }
    }

    private static void AddTitle(string title, Body body)
    {
        body.AppendChild(CreateParagraph(title, Title));
        AddEmptyLine(body);
    }

    private static void AddEmptyLine(Body body)
    {
        body.AppendChild(CreateParagraph(string.Empty, SourceHeader));
    }

    private static void AddDocumentData(int id, ReportDocumentData reportData, Body body)
    {
        body.AppendChild(CreateParagraph(GetSuspiciousDocumentHeader(id), SourceHeader));
        body.AppendChild(CreateParagraph(GetSuspiciousDocumentData(reportData), SuspiciousHeader));
    }

    private static void AddSourceParagraph(ReportParagraphsData paragraphsData, int id, Body body)
    {
        body.AppendChild(CreateParagraph(GetSourceParagraphHeader(id + 1), SourceHeader));
        body.AppendChild(CreateParagraph(paragraphsData.Content, Source));
    }

    private static void AddSuspiciousParagraphs(ReportParagraphsData paragraphsData, Body body)
    {
        for (var j = 0; j < paragraphsData.ParagraphsData.Count; j++)
        {
            var reportParagraphData = paragraphsData.ParagraphsData[j];
            
            body.AppendChild(CreateParagraph(
                GetSuspiciousParagraphHeader(j + 1, reportParagraphData.LocalId), SuspiciousHeader));
            
            body.AppendChild(CreateParagraph(
                GetSuspiciousParagraphHeaderData(reportParagraphData), SuspiciousHeaderData));
            
            body.AppendChild(CreateParagraph(reportParagraphData.Content, Suspicious));
        }
    }

    private static string GetSuspiciousDocumentHeader(int id)
    {
        return $"Подозрительный документ №{id}";
    }
    
    private static string GetSuspiciousDocumentData(ReportDocumentData reportData)
    {
        return $"Имя: {reportData.Name} [{reportData.Id}]. Сходство: {reportData.Similarity:F}";
    }
    
    private static string GetSuspiciousParagraphHeader(int id, int globalId)
    {
        return $"Подозрительный параграф №{id} [{globalId}]";
    }
    
    private static string GetSuspiciousParagraphHeaderData(ReportParagraphData reportParagraphData)
    {
        return $"Сходство: {reportParagraphData.Similarity:F}. " +
               $"Документ: {reportParagraphData.DocumentName} [{reportParagraphData.DocumentId}]";
    }
    
    private static string GetSourceParagraphHeader(int id)
    {
        return $"Исходный параграф №{id}";
    }

    private static Paragraph CreateParagraph(string text, RunProperties runProperties)
    {
        var paragraph = new Paragraph();
        var run = paragraph.AppendChild(new Run());
        run.AppendChild(runProperties);
        run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(text));
        return paragraph;
    }

    private static Body CreateDocumentBody(WordprocessingDocument document)
    {
        var mainPart = document.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = mainPart.Document.AppendChild(new Body());
        return body;
    }

    private static string GetFileName(string documentNameSaved)
    {
        var dotIndex = documentNameSaved.IndexOf('.');
        var documentName = dotIndex == -1 ? UnknownName() : documentNameSaved[..dotIndex];
        return string.Format(NamePattern, documentName);
    }

    private static string UnknownName() => $"unknown_{Guid.NewGuid()}";
}