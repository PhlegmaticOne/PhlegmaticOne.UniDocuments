namespace UniDocuments.Text.Services.Preprocessing.StopWords;

public class StringComparerOrdinalIgnoreCase : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public int GetHashCode(string obj)
    {
        return obj.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}