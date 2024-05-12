namespace UniDocuments.App.Domain.Services.Documents;

public class DocumentSaveRequest
{
    public Stream DocumentStream { get; }
    public string FileName { get; }
    public Guid ProfileId { get; }
    public Guid ActivityId { get; }

    public DocumentSaveRequest(Guid profileId, Guid activityId, Stream documentStream, string fileName)
    {
        ProfileId = profileId;
        ActivityId = activityId;
        DocumentStream = documentStream;
        FileName = fileName;
    }
}