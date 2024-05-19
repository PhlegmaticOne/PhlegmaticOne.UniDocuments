namespace UniDocuments.Text.Domain.Services.Reports.Models;

public class ReportParagraphsData
{
    public string Content { get; }
    public List<ReportParagraphData> ParagraphsData { get; }

    public ReportParagraphsData(string content)
    {
        Content = content;
        ParagraphsData = new List<ReportParagraphData>();
    }

    public void AddParagraphData(ReportParagraphData paragraphData)
    {
        ParagraphsData.Add(paragraphData);
    }
}