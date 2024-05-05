namespace UniDocuments.Text.Domain.Providers.Reports.Data.Models;

public class PlagiarismReportData
{
    public PlagiarismReportData(
        Guid documentId, string documentName,
        List<ReportDocumentData> documentData, 
        List<ReportParagraphsData> paragraphsData)
    {
        DocumentData = documentData;
        ParagraphsData = paragraphsData;
        DocumentId = documentId;
        DocumentName = documentName;
    }
    
    public Guid DocumentId { get; }
    public string DocumentName { get; }
    public List<ReportDocumentData> DocumentData { get; }
    public List<ReportParagraphsData> ParagraphsData { get; }
}