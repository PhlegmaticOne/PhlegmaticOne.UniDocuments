namespace UniDocuments.Text.Domain.Providers.Reports;

public class PlagiarismReport
{
    public Stream ResponseStream { get; }
    public string ContentType { get; }
    public string Name { get; }

    public PlagiarismReport(Stream responseStream, string contentType, string name)
    {
        ResponseStream = responseStream;
        ContentType = contentType;
        Name = name;
    }
}