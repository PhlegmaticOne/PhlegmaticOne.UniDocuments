namespace UniDocuments.Text.Domain.Providers.Reports;

public class PlagiarismReport
{
    public byte[] ResponseStream { get; }
    public string ContentType { get; }
    public string Name { get; }

    public PlagiarismReport(byte[] responseStream, string contentType, string name)
    {
        ResponseStream = responseStream;
        ContentType = contentType;
        Name = name;
    }
}