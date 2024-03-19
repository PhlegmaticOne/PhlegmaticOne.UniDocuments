﻿namespace UniDocuments.App.Domain.FileStorage;

public record FileLoadResponse(Guid FileId, string FileName, Stream? FileStream)
{
    public static FileLoadResponse NoContent()
    {
        return new FileLoadResponse(Guid.Empty, string.Empty, null);
    }
}
