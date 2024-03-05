namespace UniDocuments.Text.Algorithms;

public static class CollectionExtensions
{
    public static T GetMinInRange<T>(this IList<T> list, int start, int end) where T : IComparable<T>
    {
        if (list.Count == 0)
        {
            throw new ArgumentException(nameof(list));
        }
        
        var min = list[start];

        for (var i = start + 1; i < end; i++)
        {
            var item = list[i];
            
            if (item.CompareTo(min) < 0)
            {
                min = item;
            }
        }

        return min;
    }

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