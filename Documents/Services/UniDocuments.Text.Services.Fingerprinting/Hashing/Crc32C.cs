namespace UniDocuments.Text.Services.Fingerprinting.Hashing;

public static class Crc32C
{
    public static uint Compute(in ReadOnlySpan<char> input, int offset, int length)
    {
        if (offset < 0 || length < 0 || offset + length > input.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }
        
        return length > 0 ? Crc32CHashCalculator.AppendHash(0, input, offset, length) : 0;
    }
}