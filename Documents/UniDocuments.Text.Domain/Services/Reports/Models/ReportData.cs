namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportData
{
    public ReportData(
        ReportDocumentData sourceData,
        List<ReportDocumentData> documentData, 
        List<ReportParagraphsData> paragraphsData)
    {
        SourceData = sourceData;
        DocumentData = documentData;
        ParagraphsData = paragraphsData;
    }
    
    public ReportDocumentData SourceData { get; }
    public List<ReportDocumentData> DocumentData { get; }
    public List<ReportParagraphsData> ParagraphsData { get; }
}