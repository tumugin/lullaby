namespace Lullaby.Common.Utils;

public static class LinqExtensions
{
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class =>
        enumerable.Where(e => e != null).Select(e => e!);
}
