namespace UniDocuments.Text.Domain.Extensions;

public static class Extensions
{
    public static List<T> ToList<T>(this T item)
    {
        return new List<T> { item };
    }
}