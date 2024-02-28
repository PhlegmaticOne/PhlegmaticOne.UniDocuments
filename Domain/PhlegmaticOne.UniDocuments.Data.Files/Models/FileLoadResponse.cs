namespace PhlegmaticOne.UniDocuments.Data.Files.Models;

public class FileLoadResponse
{
    private readonly Stream _fileStream;
    public string FileName { get; }
    public Guid FileId { get; }

    public FileLoadResponse(Guid fileId, string fileName, Stream fileStream)
    {
        FileId = fileId;
        FileName = fileName;
        _fileStream = fileStream;
    }

    public Stream GetFileStream() => _fileStream;
}
