namespace UniDocuments.Text.Domain.Extensions;

public static class StreamExtensions
{
    public static void SeekToZero(this Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}