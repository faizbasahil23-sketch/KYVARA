namespace Kyvara.SharedKernel.Pagination;

public sealed class PageResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
        = Array.Empty<T>();

    public int Page { get; init; }

    public int Size { get; init; }

    public long TotalItems { get; init; }

    public int TotalPages =>
        Size==0
        ? 0
        : (int)Math.Ceiling((double)TotalItems/Size);
}
