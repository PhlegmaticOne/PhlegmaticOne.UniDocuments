namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportParagraphData
{
    public ReportParagraphData(string content, ReportDocumentData documentData)
    {
        Content = content;
        DocumentData = documentData;
    }
    
    public string Content { get; }
    public ReportDocumentData DocumentData { get; set; }
}