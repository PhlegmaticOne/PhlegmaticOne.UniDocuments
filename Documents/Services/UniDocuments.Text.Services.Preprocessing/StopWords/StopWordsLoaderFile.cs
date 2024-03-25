using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.Text.Services.Preprocessing.StopWords;

public class StopWordsLoaderFile : IStopWordsLoader
{
    private const string StopWordsPath = @"StopWords\Data\english.txt";
    public Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken)
    {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var directoryPath = directory.Parent!.Parent!.Parent!.FullName;
        var path = Path.Combine(directoryPath, StopWordsPath);
        return File.ReadAllLinesAsync(path, cancellationToken);
    }
}