namespace Kyvara.SharedKernel.Pagination;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        PageRequest request)
    {
        return query
            .Skip(request.Skip)
            .Take(request.Size);
    }
}
