namespace Lullaby.Admin.Utils;

public static class LinqPaginationExtensions
{
    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source, int perPage, int page) =>
        source.Skip(perPage * (page - 1))
            .Take(perPage);
}
