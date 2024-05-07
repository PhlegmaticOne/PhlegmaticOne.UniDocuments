namespace UniDocuments.Text.Domain.Services.Reports;

public class ReportResponse
{
    public byte[] ResponseStream { get; }
    public string ContentType { get; }
    public string Name { get; }

    public ReportResponse(byte[] responseStream, string contentType, string name)
    {
        ResponseStream = responseStream;
        ContentType = contentType;
        Name = name;
    }
}