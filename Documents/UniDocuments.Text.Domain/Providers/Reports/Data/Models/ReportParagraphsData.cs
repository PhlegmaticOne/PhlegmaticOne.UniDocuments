using UniDocuments.Text.Domain.Providers.Comparing.Responses;

namespace UniDocuments.Text.Domain.Providers.Reports.Data.Models;

public class ReportParagraphsData
{
    public string Content { get; }
    public List<ReportParagraphData> ParagraphsData { get; }

    public ReportParagraphsData(string content)
    {
        Content = content;
        ParagraphsData = new List<ReportParagraphData>();
    }

    public void AddParagraphData(Guid documentId, string documentName, CompareTextResult compareTextResult)
    {
        var data = ReportParagraphData
            .WithDocumentData(documentId, documentName)
            .FillFromCompareResult(compareTextResult);
        
        ParagraphsData.Add(data);   
    }
}