﻿namespace UniDocuments.Text.Processing.StopWords;

public class StopWordsLoaderFile : IStopWordsLoader
{
    private const string StopWordsPath = @"Content\english.txt";
    public Task<string[]> LoadStopWordsAsync(CancellationToken cancellationToken)
    {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var directoryPath = directory.Parent!.Parent!.Parent!.FullName;
        var path = Path.Combine(directoryPath, StopWordsPath);
        return File.ReadAllLinesAsync(path, cancellationToken);
    }
}