namespace PhlegmaticOne.UniDocuments.Data.Files.Models;

public class FileLocalSaveRequest
{
    public string LocalFilePath { get; }
    public string FileName { get; }

    public FileLocalSaveRequest(string localFilePath, string fileName)
    {
        LocalFilePath = localFilePath;
        FileName = fileName;
    }
}
