using System.Drawing;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using UniDocuments.Text.Domain.Providers.Reports;
using UniDocuments.Text.Domain.Providers.Reports.Data.Models;

namespace UniDocuments.Text.Services.Reporting;

public class PlagiarismReportProviderPdf : IPlagiarismReportProvider
{
    private const string ContentType = "application/pdf";
    private const string NamePattern = "report_{0}.pdf";
    private const float In = 72;

    private static readonly TextFormat SourceHighlighted = new()
    {
        Font = StandardFonts.Times,
        FontSize = 14,
        BackColor = Color.Green,
        ForeColor = Color.Black
    };
    
    private static readonly TextFormat SuspiciousHighlighted = new()
    {
        Font = StandardFonts.Times,
        FontSize = 14,
        BackColor = Color.Red,
        ForeColor = Color.Black
    };
    
    private static readonly TextFormat Header = new()
    {
        Font = StandardFonts.Times,
        FontSize = 20,
        FontBold = true,
        ForeColor = Color.Black
    };
    
    private static readonly TextFormat Default = new()
    {
        Font = StandardFonts.Times,
        FontSize = 14
    };

    public Task<PlagiarismReport> BuildReportAsync(
        PlagiarismReportData reportData, CancellationToken cancellationToken)
    {
        return Task.Run(() => BuildReport(reportData), cancellationToken);
    }

    private static PlagiarismReport BuildReport(PlagiarismReportData reportData)
    {
        var document = new GcPdfDocument();
        
        SetTitle(document, reportData);

        var layout = CreateLayout(document);
        
        AppendDocuments(layout, reportData);
        
        AppendParagraphs(layout, reportData);

        DrawText(document, layout);
        
        return ToReport(document, reportData);
    }

    private static PlagiarismReport ToReport(GcPdfDocument document, PlagiarismReportData reportData)
    {
        var stream = new MemoryStream();
        document.Save(stream);
        //var name = string.Format(NamePattern, reportData.DocumentName);
        return new PlagiarismReport(stream, ContentType, "name.pdf");
    }

    private static void DrawText(GcPdfDocument document, TextLayout inputLayout)
    {
        var layout = inputLayout;
        layout.PerformLayout(true);
        
        var to = new TextSplitOptions(layout)
        {
            MinLinesInFirstParagraph = 2,
            MinLinesInLastParagraph = 2
        };
        
        while (true)
        {
            var splitResult = layout.Split(to, out var rest);
            document.Pages.Add().Graphics.DrawTextLayout(layout, PointF.Empty);
            if (splitResult != SplitResult.Split)
            {
                break;
            }

            layout = rest;
        }
    }

    private static void AppendDocuments(TextLayout layout, PlagiarismReportData reportData)
    {
        layout.AppendLine("Подозрительные документы: ", Header);

        foreach (var documentData in reportData.DocumentData)
        {
            layout.AppendLine($"Документ: {documentData.Name} [{documentData.Id}]. " +
                              $"Коэффициент схожести: {documentData.Similarity}");
        }
        
        layout.AppendLine();
    }

    private static void AppendParagraphs(TextLayout layout, PlagiarismReportData reportData)
    {
        layout.AppendLine("Подозрительные параграфы: ", Header);

        foreach (var reportParagraphsData in reportData.ParagraphsData)
        {
            layout.AppendLine(reportParagraphsData.Content);

            foreach (var suspicious in reportParagraphsData.ParagraphsData)
            {
                layout.AppendLine($"Подозрительный параграф: {suspicious.Content}. " +
                                  $"Документ: {suspicious.DocumentName} [{suspicious.DocumentId}]. " +
                                  $"Коэффициент схожести: {suspicious.Similarity}");
            }
        }

        layout.AppendLine();
    }

    private static TextLayout CreateLayout(GcPdfDocument document)
    {
        var layout = new TextLayout(In)
        {
            DefaultFormat =
            {
                Font = StandardFonts.TimesItalic
            },
            MaxWidth = document.PageSize.Width - In * 2,
            MaxHeight = document.PageSize.Height,
            FirstLineIndent = In * 0.5f,
            ParagraphSpacing = In * 0.05f,
            LineSpacingScaleFactor = 0.8f,
            MarginAll = In
        };
        
        return layout;
    }

    private static void SetTitle(GcPdfDocument document, PlagiarismReportData reportData)
    {
        var title = $"Результат проверки на плагиат документа {reportData.DocumentName} [{reportData.DocumentId}]";
        document.Metadata.Title = title;
    }
}