namespace UniDocuments.Text.Services.Fingerprinting.Extensions;

public static class CollectionExtensions
{
    public static int GetMinIndexInRange<T>(this IList<T> list, int start, int end) where T : IComparable<T>
    {
        var minIndex = start;
        var minValue = list[minIndex];

        for (var i = start + 1; i < end; i++)
        {
            var item = list[i];
            
            if (item.CompareTo(minValue) < 0)
            {
                minValue = item;
                minIndex = i;
            }
        }

        return minIndex - start;
    }
}