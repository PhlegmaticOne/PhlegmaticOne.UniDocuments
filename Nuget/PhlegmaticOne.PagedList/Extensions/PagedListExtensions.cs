using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.PagedLists.Implementation;

namespace PhlegmaticOne.PagedLists.Extensions;

public static class PagedListExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex,
        int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException(
                $"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");

        var count = await source
            .CountAsync(cancellationToken);

        var items = await source
            .Skip((pageIndex - indexFrom) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            IndexFrom = indexFrom,
            TotalCount = count,
            Items = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }

    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize,
        int indexFrom = 0)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException(
                $"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");

        var count = source.Count();

        var items = source
            .Skip((pageIndex - indexFrom) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            IndexFrom = indexFrom,
            TotalCount = count,
            Items = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }

    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize,
        int indexFrom = 0)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException(
                $"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");

        if (source is IList<T> list) return ToPagedList(list, pageIndex, pageSize, indexFrom);

        return ToPagedList(source.ToList(), pageIndex, pageSize, indexFrom);
    }

    public static IPagedList<T> ToPagedList<T>(this IList<T> source, int pageIndex, int pageSize, int indexFrom = 0)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException(
                $"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");

        var count = source.Count;

        var items = source
            .Skip((pageIndex - indexFrom) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            IndexFrom = indexFrom,
            TotalCount = count,
            Items = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }
}