namespace PhlegmaticOne.PagedLists.Implementation;

public class PagedList<T> : IPagedList<T>
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public int IndexFrom { get; init; }

    public IList<T> Items { get; init; } = null!;

    public bool HasPreviousPage => PageIndex - IndexFrom > 0;

    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

    public static PagedList<TResult> From<TSource, TResult>(IPagedList<TSource> source,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
    {
        var items = new List<TResult>(converter(source.Items));
        return new PagedList<TResult>
        {
            IndexFrom = source.IndexFrom,
            Items = items,
            TotalCount = source.TotalCount,
            TotalPages = source.TotalPages,
            PageIndex = source.PageIndex,
            PageSize = source.PageSize
        };
    }

    public static PagedList<TResult> Empty<TResult>()
    {
        var items = new List<TResult>();
        return new PagedList<TResult>
        {
            Items = items
        };
    }
}